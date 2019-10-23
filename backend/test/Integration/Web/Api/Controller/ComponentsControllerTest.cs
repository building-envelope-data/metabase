using Startup = Icon.Startup;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using Xunit;
using WebApplicationFactoryClientOptions = Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions;
using IdentityModel.Client;

namespace Test.Integration.Web.Api.Controller
{
    public class ComponentsControllerTest : Base
    {
        public ComponentsControllerTest(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        private class GetListComponent
        {
            Guid Id;
        }

        [Fact]
        public async Task CanGetList()
        {
            // Act
            var httpResponse = await HttpClient.GetAsync("/api/components");

            // Assert
            httpResponse.EnsureSuccessStatusCode();
            var components = JsonSerializer.Deserialize<IEnumerable<GetListComponent>>(await httpResponse.Content.ReadAsStringAsync());
            Assert.Equal(0, components.Count());
        }

        private class PostComponent
        {
        }

        [Fact]
        public async Task CannotPostAnonymously()
        {
            // Act
            var httpResponse = await HttpClient.PostAsync("/api/components", MakeJsonHttpContent(new PostComponent()));
            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, httpResponse.StatusCode);
        }

        [Fact]
        public async Task CanPost()
        {
            // Arrange
            Factory.SeedUsers();
            var tokenResponse = await RequestAuthToken("simon@icon.com", "simonSIMON123@");
            HttpClient.SetBearerToken(tokenResponse.AccessToken);

            // Act
            var httpResponse = await HttpClient.PostAsync("/api/components", MakeJsonHttpContent(new PostComponent()));

            // Assert
            httpResponse.EnsureSuccessStatusCode();
            var id = JsonSerializer.Deserialize<Guid>(await httpResponse.Content.ReadAsStringAsync());
            Assert.Equal(Guid.NewGuid(), id);
        }
    }
}