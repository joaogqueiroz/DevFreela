using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace DevFreela.ApiTests
{
    public class ProjectsApiTests : IClassFixture<ApiFactory>
    {
        private const int UnknownId = 999_999;
        private readonly ApiFactory _factory;

        public ProjectsApiTests(ApiFactory factory)
        {
            _factory = factory;
        }

        private static StringContent Json(string body) => new StringContent(body, Encoding.UTF8, "application/json");

        private static object Card() => new { creditCardNumber = "4111111111111111", cvv = "123", expiresAt = "12/30", fullName = "Client" };

        private static async Task<int> CreateProjectAsync(HttpClient client, int clientId)
        {
            var response = await client.PostAsJsonAsync("/api/projects", new
            {
                title = "Landing page",
                description = "Build a landing page",
                idClient = clientId,
                totalCost = 1500
            });
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var projects = JsonDocument.Parse(await client.GetStringAsync("/api/projects")).RootElement;
            return projects.EnumerateArray().Max(p => p.GetProperty("id").GetInt32());
        }

        [Fact]
        public async Task Projects_WithoutToken_Return401()
        {
            Assert.Equal(HttpStatusCode.Unauthorized, (await _factory.CreateClient().GetAsync("/api/projects")).StatusCode);
        }

        [Fact]
        public async Task CreateProject_AsFreelancer_Returns403()
        {
            var (freelancer, freelancerId) = await _factory.SignInAsync("freelancer");

            var response = await freelancer.PostAsJsonAsync("/api/projects", new { title = "Title", description = "Description", idClient = freelancerId, totalCost = 100 });

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData(@"{ ""title"": ""Title"", ""description"": ""Description"", ""idClient"": 1, ""totalCost"": ""a lot"" }")]   // text in a decimal
        [InlineData(@"{ ""title"": ""Title"", ""description"": ""Description"", ""idClient"": ""one"", ""totalCost"": 100 }")]    // text in an integer
        [InlineData(@"{ ""title"": ""Title"", ""description"": ""Description"", ""idClient"": 1.5, ""totalCost"": 100 }")]      // decimal in an integer
        [InlineData(@"{ ""title"": [""Title""], ""description"": ""Description"", ""idClient"": 1, ""totalCost"": 100 }")]     // array in a string
        public async Task CreateProject_WrongJsonTypes_Returns400(string body)
        {
            var (client, _) = await _factory.SignInAsync("client");

            var response = await client.PostAsync("/api/projects", Json(body));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-100)]
        public async Task CreateProject_CostNotPositive_Returns400(decimal cost)
        {
            var (client, clientId) = await _factory.SignInAsync("client");

            var response = await client.PostAsJsonAsync("/api/projects", new { title = "Title", description = "Description", idClient = clientId, totalCost = cost });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("Total cost must be greater than zero", await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task GetProject_IdThatIsNotANumber_Returns400()
        {
            var (client, _) = await _factory.SignInAsync("client");

            Assert.Equal(HttpStatusCode.BadRequest, (await client.GetAsync("/api/projects/abc")).StatusCode);
        }

        [Fact]
        public async Task ActionsOnAnUnknownProject_Return404()
        {
            var (client, _) = await _factory.SignInAsync("client");

            Assert.Equal(HttpStatusCode.NotFound, (await client.GetAsync($"/api/projects/{UnknownId}")).StatusCode);
            Assert.Equal(HttpStatusCode.NotFound, (await client.PutAsJsonAsync($"/api/projects/{UnknownId}", new { id = UnknownId, title = "Title", description = "Description", totalCost = 100 })).StatusCode);
            Assert.Equal(HttpStatusCode.NotFound, (await client.PutAsync($"/api/projects/{UnknownId}/start", null)).StatusCode);
            Assert.Equal(HttpStatusCode.NotFound, (await client.PutAsJsonAsync($"/api/projects/{UnknownId}/finish", Card())).StatusCode);
            Assert.Equal(HttpStatusCode.NotFound, (await client.DeleteAsync($"/api/projects/{UnknownId}")).StatusCode);
        }

        [Fact]
        public async Task Comment_OnAnUnknownProject_Returns404()
        {
            var (client, userId) = await _factory.SignInAsync("client");

            var response = await client.PostAsJsonAsync($"/api/projects/{UnknownId}/comments", new { content = "Hello", idUser = userId });

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Finish_ProjectThatWasNeverStarted_Returns400()
        {
            var (client, clientId) = await _factory.SignInAsync("client");
            var projectId = await CreateProjectAsync(client, clientId);

            var response = await client.PutAsJsonAsync($"/api/projects/{projectId}/finish", Card());

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("Only a project in progress can be finished.", await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task Finish_Twice_SecondCallIsRefused()
        {
            var (client, clientId) = await _factory.SignInAsync("client");
            var projectId = await CreateProjectAsync(client, clientId);
            await client.PutAsync($"/api/projects/{projectId}/start", null);

            var first = await client.PutAsJsonAsync($"/api/projects/{projectId}/finish", Card());
            var second = await client.PutAsJsonAsync($"/api/projects/{projectId}/finish", Card());

            Assert.Equal(HttpStatusCode.NoContent, first.StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, second.StatusCode);
        }

        [Fact]
        public async Task Comment_OnAnExistingProject_UsesTheProjectFromTheRoute()
        {
            var (client, clientId) = await _factory.SignInAsync("client");
            var projectId = await CreateProjectAsync(client, clientId);

            // idProject in the body is ignored; the route decides which project gets the comment
            var response = await client.PostAsJsonAsync($"/api/projects/{projectId}/comments", new { content = "Looks good", idUser = clientId, idProject = UnknownId });

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
