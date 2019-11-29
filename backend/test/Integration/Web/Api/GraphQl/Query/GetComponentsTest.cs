// Inspired by https://chillicream.com/blog/2019/04/11/integration-tests
// TODO When mature, use the client https://github.com/ChilliCream/hotchocolate/issues/1011

using System.Collections.Generic;
using System.Linq;
using ValueObjects = Icon.ValueObjects;
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
using Models = Icon.Models;

namespace Test.Integration.Web.Api.GraphQl.Query
{
    public class GetComponentsTest : GraphQlTestBase
    {
        public GetComponentsTest(CustomWebApplicationFactory factory) : base(factory) { }

        /* [Fact] */
        /* public async Task Anonymously() */
        /* { */
        /*   // Act */
        /*   var httpResponse = await ComponentsClient.Post.Raw(); */
        /*   // Assert */
        /*   httpResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized); */
        /* } */

        [Fact]
        public async Task Empty()
        {
            // Act
            var components = await Client.GetComponentsSuccessfully();
            // Assert
            components.Should().BeEmpty();
        }

        [Fact]
        public async Task Single()
        {
            // Arrange
            var component = await Client.CreateComponentSuccessfully(
                new GraphQlClient.ComponentInputData(
                  name: "Component A",
                  abbreviation: "C!A",
                  description: "Best component ever!",
                  availableFrom: null,
                  availableUntil: null,
                  categories: new ValueObjects.ComponentCategory[0] { }
                  )
                );
            // Act
            var components = await Client.GetComponentsSuccessfully();
            // Assert
            components
              .Should().NotBeEmpty()
              .And.HaveCount(1)
              .And.BeEquivalentTo(component);
        }

        [Fact]
        public async Task Multiple()
        {
            // Arrange
            var component1 = await Client.CreateComponentSuccessfully(
                new GraphQlClient.ComponentInputData(
                  name: "Component A",
                  abbreviation: "C!A",
                  description: "Best component ever!",
                  availableFrom: null,
                  availableUntil: null,
                  categories: new ValueObjects.ComponentCategory[0] { }
                  )
                );
            var component2 = await Client.CreateComponentSuccessfully(
            new GraphQlClient.ComponentInputData(
              name: "Component B",
              abbreviation: "C!B",
              description: "Second best component ever!",
              availableFrom: DateTime.UtcNow,
              availableUntil: null,
              categories: new ValueObjects.ComponentCategory[2]
              {
                ValueObjects.ComponentCategory.Layer,
                ValueObjects.ComponentCategory.Unit
              }
              )
                );
            var component3 = await Client.CreateComponentSuccessfully(
            new GraphQlClient.ComponentInputData(
              name: "Component C",
              abbreviation: "C!C",
              description: "Third best component ever!",
              availableFrom: null,
              availableUntil: DateTime.UtcNow,
              categories: new ValueObjects.ComponentCategory[2]
              {
                ValueObjects.ComponentCategory.Material,
                ValueObjects.ComponentCategory.Layer
              }
              )
                );
            var component4 = await Client.CreateComponentSuccessfully(
            new GraphQlClient.ComponentInputData(
              name: "Component D",
              abbreviation: "C!D",
              description: "Fourth best component ever!",
              availableFrom: null,
              availableUntil: null,
              categories: new ValueObjects.ComponentCategory[0] { }
              )
                );
            var component5 = await Client.CreateComponentSuccessfully(
            new GraphQlClient.ComponentInputData(
              name: "Component E",
              abbreviation: "C!E",
              description: "Fifth best component ever!",
              availableFrom: DateTime.UtcNow.AddDays(2),
              availableUntil: DateTime.UtcNow.AddDays(10),
              categories: new ValueObjects.ComponentCategory[1]
              {
                ValueObjects.ComponentCategory.Material
              }
              )
                );
            // Act
            var components = await Client.GetComponentsSuccessfully();
            // Assert
            components
              .Should().NotBeEmpty()
              .And.HaveCount(5)
              .And.BeEquivalentTo(
                  component1,
                  component2,
                  component3,
                  component4,
                  component5
                  );
        }
    }
}