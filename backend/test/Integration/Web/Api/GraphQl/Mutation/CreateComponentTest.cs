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
  public class CreateComponentTest : TestBase
  {
    public CreateComponentTest(CustomWebApplicationFactory factory) : base(factory) { }

    /* [Fact] */
    /* public async Task Anonymously() */
    /* { */
    /*   // Act */
    /*   var httpResponse = await ComponentsClient.Post.Raw(); */
    /*   // Assert */
    /*   httpResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized); */
    /* } */

    public class AuthorizedData : ResponseBase
    {
      public CreateComponentData createComponent { get; set; }

      public class CreateComponentData : ResponseBase
      {
        public Guid id { get; set; }
        public DateTime timestamp { get; set; }
      }
    }

    [Fact]
      public async Task Authorized()
      {
        // Arrange
        // TODO Instead of seeds we should use the public API as much as possible. Otherwise the state of the database may not be a possible state in production.
        await Factory.SeedUsers();
        Factory.SeedAuth();
        await Authorize(HttpClient, "simon@icon.com", "simonSIMON123@");
        // Act
        var beforeTimestamp = DateTime.UtcNow;
        var httpResponse = await HttpClient.PostAsync(
            "/graphql",
            MakeJsonHttpContent(
              new Request() {
                query =
                  @"mutation {
                    createComponent {
                      id
                      timestamp
                    }
                  }"
                }
              )
            );
        // Assert
        httpResponse.EnsureSuccessStatusCode();
        var response = await new ResponseParser().Parse<Response<AuthorizedData>>(httpResponse);
        response.EnsureNoOverflow();
        response.EnsureNoErrors();
        response.data.EnsureNoOverflow();
        response.data.createComponent.id
          .Should().NotBeEmpty();
        response.data.createComponent.timestamp
          .Should().BeAfter(beforeTimestamp)
          .And.BeBefore(DateTime.UtcNow);
      }
  }
}
