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
        public ComponentsControllerTest(CustomWebApplicationFactory factory) : base(factory) { }

				public class ComponentsBase : Base
				{
						protected Components.Client ComponentsClient { get; }

						public ComponentsBase(CustomWebApplicationFactory factory) : base(factory)
						{
								ComponentsClient = new Components.Client(HttpClient);
						}
				}

        public class GetListTest : ComponentsBase
        {
					public GetListTest(CustomWebApplicationFactory factory) : base(factory) {
					}

            [Fact]
            public async Task WhenEmpty()
            {
                // Act
                var components = await ComponentsClient.List.Deserialized();
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
                var component = new Components.ListClient.Output { Id = await ComponentsClient.Post.Deserialized() };
                // Act
                var components = await ComponentsClient.List.Deserialized();
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
                var component1 = new Components.ListClient.Output { Id = await ComponentsClient.Post.Deserialized() };
                var component2 = new Components.ListClient.Output { Id = await ComponentsClient.Post.Deserialized() };
                var component3 = new Components.ListClient.Output { Id = await ComponentsClient.Post.Deserialized() };
                var component4 = new Components.ListClient.Output { Id = await ComponentsClient.Post.Deserialized() };
                var component5 = new Components.ListClient.Output { Id = await ComponentsClient.Post.Deserialized() };
                // Act
                var components = await ComponentsClient.List.Deserialized();
                // Assert
                components.Should().BeEquivalentTo(component1, component2, component3, component4, component5);
            }
        }

        public class PostTest : ComponentsBase
        {
            public PostTest(CustomWebApplicationFactory factory) : base(factory) { }

            [Fact]
            public async Task CannotAnonymously()
            {
                // Act
                var httpResponse = await ComponentsClient.Post.Raw();
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
                var id = await ComponentsClient.Post.Deserialized();
                // Assert
                id.Should().NotBeEmpty();
            }
        }
    }
}