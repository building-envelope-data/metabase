using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Snapshooter.NUnit;

namespace Metabase.Tests.Integration.GraphQl;

[TestFixture]
public sealed class GraphQlSchemaTests
    : IntegrationTests
{
    [Test]
    [SuppressMessage("Naming", "CA1707")]
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