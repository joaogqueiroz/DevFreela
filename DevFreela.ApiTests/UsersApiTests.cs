using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace DevFreela.ApiTests
{
    public class UsersApiTests : IClassFixture<ApiFactory>
    {
        private readonly ApiFactory _factory;
        private readonly HttpClient _client;

        public UsersApiTests(ApiFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        private static StringContent Json(string body) => new StringContent(body, Encoding.UTF8, "application/json");

        [Fact]
        public async Task SignUp_ValidData_Returns201WithoutThePassword()
        {
            var response = await _client.PostAsJsonAsync("/api/users", new
            {
                fullName = "Client User",
                email = $"{Guid.NewGuid():N}@test.com",
                password = "Senha@123",
                birthDate = "1990-01-01",
                role = "client"
            });

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.DoesNotContain("Senha@123", await response.Content.ReadAsStringAsync());
        }

        [Theory]
        [InlineData(@"{ ""fullName"": 123, ""email"": ""a@test.com"", ""password"": ""Senha@123"", ""birthDate"": ""1990-01-01"", ""role"": ""client"" }")]
        [InlineData(@"{ ""fullName"": ""User"", ""email"": ""a@test.com"", ""password"": ""Senha@123"", ""birthDate"": ""not a date"", ""role"": ""client"" }")]
        [InlineData(@"{ ""fullName"": ""User"", ""email"": ""a@test.com"", ""password"": ""Senha@123"", ""birthDate"": 19900101, ""role"": ""client"" }")]
        [InlineData(@"{ ""fullName"": ""User"", ""email"": ""a@test.com"", ""password"": ""Senha@123"", ""birthDate"": ""1990-01-01"", ""role"": true }")]
        public async Task SignUp_WrongJsonTypes_Returns400(string body)
        {
            var response = await _client.PostAsync("/api/users", Json(body));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData(@"{ ""fullName"": ""User"", ")]
        [InlineData(@"")]
        public async Task SignUp_MalformedOrEmptyBody_Returns400(string body)
        {
            var response = await _client.PostAsync("/api/users", Json(body));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task SignUp_InvalidFields_Returns400WithTheMessages()
        {
            var response = await _client.PostAsJsonAsync("/api/users", new
            {
                fullName = "",
                email = "not-an-email",
                birthDate = "2999-01-01",
                role = "admin"
            });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var body = await response.Content.ReadAsStringAsync();
            Assert.Contains("Name is required", body);
            Assert.Contains("Wrong e-mail", body);
            Assert.Contains("Password should have", body);
            Assert.Contains("Birth date must be in the past", body);
            Assert.Contains("Role must be client or freelancer", body);
        }

        [Fact]
        public async Task Login_WrongPassword_Returns400()
        {
            var email = $"{Guid.NewGuid():N}@test.com";
            await _client.PostAsJsonAsync("/api/users", new { fullName = "User", email, password = "Senha@123", birthDate = "1990-01-01", role = "client" });

            var response = await _client.PutAsJsonAsync("/api/users/login", new { email, password = "Errada@123" });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Login_UnknownEmail_Returns400()
        {
            var response = await _client.PutAsJsonAsync("/api/users/login", new { email = $"{Guid.NewGuid():N}@test.com", password = "Senha@123" });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetUser_WithoutToken_Returns401()
        {
            Assert.Equal(HttpStatusCode.Unauthorized, (await _client.GetAsync("/api/users/1")).StatusCode);
        }
    }
}
