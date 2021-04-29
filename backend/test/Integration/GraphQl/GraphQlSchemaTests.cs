using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Snapshooter.Xunit;
using Xunit;

namespace Metabase.Tests.Integration.GraphQl
{
    public sealed class GraphQlSchemaTests
      : IntegrationTests
    {
        [Fact]
        public async Task IsUnchanged()
        {
            // Act
            var response = await HttpClient.GetAsync("/graphql?sdl").ConfigureAwait(false);
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var schema = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            Snapshot.Match(schema);
        }
    }
}