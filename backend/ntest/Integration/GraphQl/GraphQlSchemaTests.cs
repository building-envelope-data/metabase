using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Snapshooter.NUnit;
using NUnit.Framework;

namespace Metabase.Tests.Integration.GraphQl
{
    [TestFixture]
    public sealed class GraphQlSchemaTests
      : IntegrationTests
    {
        [Test]
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