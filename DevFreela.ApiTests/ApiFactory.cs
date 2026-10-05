using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using DevFreela.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using Testcontainers.RabbitMq;

namespace DevFreela.ApiTests
{
    // Runs the real API in memory against SQL Server and RabbitMQ in containers. The API does not
    // migrate on startup, so the factory applies the EF Core migrations before the tests run.
    public class ApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly MsSqlContainer _sqlServer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .Build();

        // RabbitMQ refuses the guest user from outside localhost, which is how the mapped port looks
        private readonly RabbitMqContainer _rabbitMq = new RabbitMqBuilder()
            .WithImage("rabbitmq:3.13-management")
            .WithUsername("devfreela")
            .WithPassword("devfreela")
            .Build();

        public async Task InitializeAsync()
        {
            await Task.WhenAll(_sqlServer.StartAsync(), _rabbitMq.StartAsync());

            using var scope = Services.CreateScope();
            scope.ServiceProvider.GetRequiredService<DevFreelaDbContext>().Database.Migrate();
        }

        public new async Task DisposeAsync()
        {
            await base.DisposeAsync();
            await Task.WhenAll(_sqlServer.DisposeAsync().AsTask(), _rabbitMq.DisposeAsync().AsTask());
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var connectionString = new SqlConnectionStringBuilder(_sqlServer.GetConnectionString())
            {
                InitialCatalog = "DevFreelaApiTests"
            }.ConnectionString;

            builder.UseSetting("ConnectionStrings:DevFreelaCs", connectionString);
            builder.UseSetting("RabbitMQ:HostName", _rabbitMq.Hostname);
            builder.UseSetting("RabbitMQ:Port", _rabbitMq.GetMappedPublicPort(5672).ToString());
            builder.UseSetting("RabbitMQ:UserName", "devfreela");
            builder.UseSetting("RabbitMQ:Password", "devfreela");
        }

        // Signs up a user with the given role and returns a client carrying their token, plus their id.
        public async Task<(HttpClient Client, int UserId)> SignInAsync(string role)
        {
            var client = CreateClient();
            var email = $"{Guid.NewGuid():N}@test.com";
            var created = await client.PostAsJsonAsync("/api/users", new
            {
                fullName = $"Test {role}",
                email,
                password = "Senha@123",
                birthDate = "1990-01-01",
                role
            });
            created.EnsureSuccessStatusCode();
            var userId = JsonDocument.Parse(await created.Content.ReadAsStringAsync()).RootElement.GetProperty("id").GetInt32();

            var login = await client.PutAsJsonAsync("/api/users/login", new { email, password = "Senha@123" });
            login.EnsureSuccessStatusCode();
            var token = JsonDocument.Parse(await login.Content.ReadAsStringAsync()).RootElement.GetProperty("token").GetString();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return (client, userId);
        }
    }
}
