// Inspired by https://chillicream.com/blog/2019/04/11/integration-tests
// TODO When mature, use the client https://github.com/ChilliCream/hotchocolate/issues/1011

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Icon;
using IdentityModel.Client;
using Xunit;
using HttpResponse = Microsoft.AspNetCore.Http.HttpResponse;
using Models = Icon.Models;
using ValueObjects = Icon.ValueObjects;
using WebApplicationFactoryClientOptions = Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions;

namespace Test.Integration.Web.Api.GraphQl.Mutation
{
    public class CreateComponentTest : GraphQlTestBase
    {
        public CreateComponentTest(CustomWebApplicationFactory factory) : base(factory) { }

        [Fact]
        public async Task Anonymously()
        {
            // Act
            var errors = await Client.CreateComponentErroneously(
                new GraphQlClient.CreateComponentInput(
                  new GraphQlClient.ComponentInformationInput(
                    name: "Component A",
                    abbreviation: "C!A",
                    description: "Best component ever!",
                    availableFrom: null,
                    availableUntil: null,
                    categories: new ValueObjects.ComponentCategory[0] { }
                    )
                )
                );
            // Assert
            errors.Count.Should().Be(1);
            errors[0]?.extensions?.code?.Should().Be("AUTH_NOT_AUTHENTICATED");
        }

        [Fact]
        public async Task Authorized()
        {
            // Arrange
            // TODO Instead of seeds we should use the public API as much as possible. Otherwise the state of the database may not be a possible state in production.
            await Factory.SeedUsers();
            Factory.SeedAuth();
            await Authorize(HttpClient, "simon@icon.com", "simonSIMON123@");
            var beforeTimestamp = DateTime.UtcNow;
            // Act
            var componentPayload = await Client.CreateComponentSuccessfully(
              new GraphQlClient.CreateComponentInput(
                  new GraphQlClient.ComponentInformationInput(
                name: "Component A",
                abbreviation: "C!A",
                description: "Best component ever!",
                availableFrom: null,
                availableUntil: null,
                categories: new ValueObjects.ComponentCategory[0] { }
                )
                )
              );
            // Assert
            componentPayload.requestTimestamp
              .Should().BeAfter(beforeTimestamp)
              .And.BeBefore(DateTime.UtcNow);
            componentPayload.component.Should().NotBeNull();
            var component = componentPayload.component!;
            component.id.Should().NotBeEmpty();
            component.timestamp
              .Should().BeAfter(beforeTimestamp)
              .And.BeBefore(DateTime.UtcNow);
            component.requestTimestamp
              .Should().Be(component.timestamp);
        }
    }
}