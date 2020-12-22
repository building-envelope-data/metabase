using System;
using System.Net;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Snapshooter.Xunit;
using Xunit;

namespace Metabase.Tests.Integration.GraphQl
{
    public sealed class GraphQlSchemaTests
      : IntegrationTests
    {
      public GraphQlSchemaTests(CustomWebApplicationFactory factory)
        : base(factory)
      { }

        [Fact]
        public async Task Is_Approved_GraphQL_Schema()
        {
            // Arrange
            // ...

            // Act
            var response = await HttpClient.GetAsync("/graphql?sdl");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var schema = await response.Content.ReadAsStringAsync();
            Snapshot.Match(schema);
        }
    }
}
