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
  public class GetComponentsTest : TestBase
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

    public class Data : ResponseBase
    {
      public IEnumerable<ComponentData> components { get; set; }

      public class ComponentData : ResponseBase
      {
        public Guid id { get; set; }
      }
    }

    [Fact]
      public async Task Empty()
      {
        // Act
        var response = await GetComponents();
        // Assert
        response.data.components
          .Should().BeEmpty();
      }

    [Fact]
      public async Task Single()
      {
        // Arrange
        var component = new Data.ComponentData { id = await CreateComponent() };
        // Act
        var response = await GetComponents();
        // Assert
        response.data.components
          .Should().NotBeEmpty()
          .And.HaveCount(1)
          .And.BeEquivalentTo(component);
      }

    [Fact]
      public async Task Multiple()
      {
        // Arrange
        var component1 = new Data.ComponentData { id = await CreateComponent() };
        var component2 = new Data.ComponentData { id = await CreateComponent() };
        var component3 = new Data.ComponentData { id = await CreateComponent() };
        var component4 = new Data.ComponentData { id = await CreateComponent() };
        var component5 = new Data.ComponentData { id = await CreateComponent() };
        // Act
        var response = await GetComponents();
        // Assert
        response.data.components
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

    private async Task<Response<Data>> GetComponents()
    {
        // Act
        var httpResponse = await HttpClient.PostAsync(
            "/graphql",
            MakeJsonHttpContent(
              new Request() {
                query =
                  @"query {
                    components {
                      id
                    }
                  }"
                }
              )
            );
        // Assert
        httpResponse.EnsureSuccessStatusCode();
        var response = await new ResponseParser().Parse<Response<Data>>(httpResponse);
        response.EnsureNoOverflow();
        response.EnsureNoErrors();
        response.data.EnsureNoOverflow();
        return response;
    }

    public class XYData : ResponseBase
    {
      public CreateComponentData createComponent { get; set; }

      public class CreateComponentData : ResponseBase
      {
        public Guid id { get; set; }
      }
    }

    private async Task<Guid> CreateComponent()
    {
        var httpResponse = await HttpClient.PostAsync(
            "/graphql",
            MakeJsonHttpContent(
              new Request() {
                query =
                  @"mutation {
                    createComponent {
                      id
                    }
                  }"
                }
              )
            );
        httpResponse.EnsureSuccessStatusCode();
        var response = await new ResponseParser().Parse<Response<XYData>>(httpResponse);
        return response.data.createComponent.id;
    }
  }
}
