using System;
using Xunit;
using Startup = Icon.Startup;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;

namespace Test.Integration.Web.Api.Controller
{
    public class ComponentsControllerTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public ComponentsControllerTest(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CanGetList()
        {
            var httpResponse = await _client.GetAsync("/api/components");
            httpResponse.EnsureSuccessStatusCode();

            var jsonResponse = JsonDocument.Parse(await httpResponse.Content.ReadAsStringAsync());
            var rootElement = jsonResponse.RootElement;
            /* Console.WriteLine(rootElement.ToString()); */
            Assert.Equal(0, rootElement.GetArrayLength());
        }
    }
}