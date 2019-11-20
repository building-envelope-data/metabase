// Inspired by https://chillicream.com/blog/2019/04/11/integration-tests
// TODO When mature, use the client https://github.com/ChilliCream/hotchocolate/issues/1011

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

namespace Test.Integration.Web.Api.GraphQl.Mutation
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
            var component = await Client.CreateComponentSuccessfully();
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
            var component1 = await Client.CreateComponentSuccessfully();
            var component2 = await Client.CreateComponentSuccessfully();
            var component3 = await Client.CreateComponentSuccessfully();
            var component4 = await Client.CreateComponentSuccessfully();
            var component5 = await Client.CreateComponentSuccessfully();
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