using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using HttpResponse = Microsoft.AspNetCore.Http.HttpResponse;
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
        public static async Task<HttpResponseMessage> GetList(HttpClient httpClient)
        {
            return await httpClient.GetAsync("/api/components");
        }

        public static async Task<HttpResponseMessage> Post(HttpClient httpClient)
        {
            return await httpClient.PostAsync("/api/components", MakeJsonHttpContent(new PostComponent()));
        }

        public ComponentsControllerTest(CustomWebApplicationFactory factory) : base(factory) { }

        private class GetListComponent
        {
            Guid Id;
        }

        public class GetListTest : Base
        {
            public GetListTest(CustomWebApplicationFactory factory) : base(factory) { }

            [Fact]
            public async Task WhenEmpty()
            {
                // Act
                var httpResponse = await GetList(HttpClient);

                // Assert
                httpResponse.EnsureSuccessStatusCode();
                var components = JsonSerializer.Deserialize<IEnumerable<GetListComponent>>(
                    await httpResponse.Content.ReadAsStringAsync()
                );
                Assert.Empty(components);
            }
        }

        private class PostComponent
        {
        }

        [Fact]
        public async Task CannotPostAnonymously()
        {
            // Act
            var httpResponse = await Post(HttpClient);
            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, httpResponse.StatusCode);
        }

        [Fact]
        public async Task CanPost()
        {
            // Arrange
            // TODO Instead of seeds we should use the public API as much as possible. Otherwise the state of the database may not be a possible state in production.
            await Factory.SeedUsers();
            Factory.SeedAuth();
            var tokenResponse = await RequestAuthToken("simon@icon.com", "simonSIMON123@");
            HttpClient.SetBearerToken(tokenResponse.AccessToken);

            // Act
            var httpResponse = await Post(HttpClient);

            // Assert
            httpResponse.EnsureSuccessStatusCode();
            var id = JsonSerializer.Deserialize<Guid>(
                await httpResponse.Content.ReadAsStringAsync()
            );
            Assert.NotNull(id);
        }
    }
}