// Inspired by https://chillicream.com/blog/2019/04/11/integration-tests
// TODO When mature, use the client https://github.com/ChilliCream/hotchocolate/issues/1011

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using ValueObjects = Metabase.ValueObjects;

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
            var components = await Client.GetComponentsSuccessfully().ConfigureAwait(false);
            // Assert
            components.Should().BeEmpty();
        }

        [Fact]
        public async Task Single()
        {
            // Arrange
            var payload = await Client.CreateComponentSuccessfully(
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
                ).ConfigureAwait(false);
            // Act
            var components = await Client.GetComponentsSuccessfully().ConfigureAwait(false);
            // Assert
            components
              .Should().NotBeEmpty()
              .And.HaveCount(1);
            // TODO Test other fields than `id` and `timestamp`
            components
              .Select(c => (c.id, c.timestamp))
              .Should().BeEquivalentTo((payload.component!.id, payload.component!.timestamp));
        }

        [Fact]
        public async Task Multiple()
        {
            // Arrange
            var payloads = new[] {
            await Client.CreateComponentSuccessfully(
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
                ).ConfigureAwait(false),
              await Client.CreateComponentSuccessfully(
                  new GraphQlClient.CreateComponentInput(
                    new GraphQlClient.ComponentInformationInput(
                      name: "Component B",
                      abbreviation: "C!B",
                      description: "Second best component ever!",
                      availableFrom: DateTime.UtcNow,
                      availableUntil: null,
                      categories: new ValueObjects.ComponentCategory[2]
                      {
                      ValueObjects.ComponentCategory.LAYER,
                      ValueObjects.ComponentCategory.UNIT
                      }
                      )
                    )
                  ).ConfigureAwait(false),
              await Client.CreateComponentSuccessfully(
                  new GraphQlClient.CreateComponentInput(
                    new GraphQlClient.ComponentInformationInput(
                      name: "Component C",
                      abbreviation: "C!C",
                      description: "Third best component ever!",
                      availableFrom: null,
                      availableUntil: DateTime.UtcNow,
                      categories: new ValueObjects.ComponentCategory[2]
                      {
                      ValueObjects.ComponentCategory.MATERIAL,
                      ValueObjects.ComponentCategory.LAYER
                      }
                      )
                    )
                  ).ConfigureAwait(false),
              await Client.CreateComponentSuccessfully(
                  new GraphQlClient.CreateComponentInput(
                    new GraphQlClient.ComponentInformationInput(
                      name: "Component D",
                      abbreviation: "C!D",
                      description: "Fourth best component ever!",
                      availableFrom: null,
                      availableUntil: null,
                      categories: new ValueObjects.ComponentCategory[0] { }
                      )
                    )
                  ).ConfigureAwait(false),
              await Client.CreateComponentSuccessfully(
                  new GraphQlClient.CreateComponentInput(
                    new GraphQlClient.ComponentInformationInput(
                      name: "Component E",
                      abbreviation: "C!E",
                      description: "Fifth best component ever!",
                      availableFrom: DateTime.UtcNow.AddDays(2),
                      availableUntil: DateTime.UtcNow.AddDays(10),
                      categories: new ValueObjects.ComponentCategory[1]
                      {
                      ValueObjects.ComponentCategory.MATERIAL
                      }
                      )
                    )
                  ).ConfigureAwait(false)
          };
            // Act
            var components = await Client.GetComponentsSuccessfully().ConfigureAwait(false);
            // Assert
            components
              .Should().NotBeEmpty()
              .And.HaveCount(5);
            // TODO Test other fields than `id` and `timestamp`
            components
              .Select(c => (c.id, c.timestamp))
              .Should().BeEquivalentTo(
                  payloads.Select(p => (p.component!.id, p.component!.timestamp))
                  );
        }
    }
}