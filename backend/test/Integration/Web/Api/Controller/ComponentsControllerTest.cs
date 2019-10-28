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
using FluentAssertions;

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
            return await httpClient.PostAsync("/api/components", MakeJsonHttpContent(new PostTest.Component()));
        }

        public ComponentsControllerTest(CustomWebApplicationFactory factory) : base(factory) { }

        public class GetListTest : Base
        {
            internal class Component
            {
                internal Guid Id;
            }

            internal static async Task<IEnumerable<Component>> Deserialize(HttpResponseMessage httpResponse)
            {
                httpResponse.EnsureSuccessStatusCode();
                return JsonSerializer.Deserialize<IEnumerable<Component>>(
                    await httpResponse.Content.ReadAsStringAsync()
                );
            }

            public GetListTest(CustomWebApplicationFactory factory) : base(factory) { }

            [Fact]
            public async Task WhenEmpty()
            {
                // Act
                var components = await Deserialize(await GetList(HttpClient));
                // Assert
                components.Should().BeEmpty();
            }

            [Fact]
            public async Task WhenSingle()
            {
                // Arrange
                await Factory.SeedUsers();
                Factory.SeedAuth();
                await Authorize(HttpClient);
                var component = new Component { Id = await PostTest.Deserialize(await Post(HttpClient)) };
                // Act
                var components = await Deserialize(await GetList(HttpClient));
                // Assert
                components.Should().BeEquivalentTo(component);
            }

            [Fact]
            public async Task WhenMultiple()
            {
                // Arrange
                await Factory.SeedUsers();
                Factory.SeedAuth();
                await Authorize(HttpClient);
                var component1 = new Component { Id = await PostTest.Deserialize(await Post(HttpClient)) };
                var component2 = new Component { Id = await PostTest.Deserialize(await Post(HttpClient)) };
                var component3 = new Component { Id = await PostTest.Deserialize(await Post(HttpClient)) };
                var component4 = new Component { Id = await PostTest.Deserialize(await Post(HttpClient)) };
                var component5 = new Component { Id = await PostTest.Deserialize(await Post(HttpClient)) };
                // Act
                var components = await Deserialize(await GetList(HttpClient));
                // Assert
                components.Should().BeEquivalentTo(component1, component2, component3, component4, component5);
            }
        }

        public class PostTest : Base
        {
            internal class Component
            {
            }

            internal static async Task<Guid> Deserialize(HttpResponseMessage httpResponse)
            {
                httpResponse.EnsureSuccessStatusCode();
                return JsonSerializer.Deserialize<Guid>(
                    await httpResponse.Content.ReadAsStringAsync()
                );
            }

            public PostTest(CustomWebApplicationFactory factory) : base(factory) { }

            [Fact]
            public async Task CannotAnonymously()
            {
                // Act
                var httpResponse = await Post(HttpClient);
                // Assert
                httpResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            }

            [Fact]
            public async Task CanAuthorized()
            {
                // Arrange
                // TODO Instead of seeds we should use the public API as much as possible. Otherwise the state of the database may not be a possible state in production.
                await Factory.SeedUsers();
                Factory.SeedAuth();
                await Authorize(HttpClient);
                // Act
                var id = await Deserialize(await Post(HttpClient));
                // Assert
                id.Should().NotBeEmpty();
            }
        }
    }
}