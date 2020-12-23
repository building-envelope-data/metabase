using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IdentityModel.Client;
using Xunit;
using TokenResponse = IdentityModel.Client.TokenResponse;
using WebApplicationFactoryClientOptions = Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions;

namespace Metabase.Tests.Integration
{
  public abstract class IntegrationTests
    {
        protected CustomWebApplicationFactory Factory { get; }
        protected HttpClient HttpClient { get; }

        protected IntegrationTests()
        {
            Factory = new CustomWebApplicationFactory();
            HttpClient = CreateHttpClient();
        }

        protected HttpClient CreateHttpClient(bool allowAutoRedirect = true)
        {
            return Factory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = allowAutoRedirect,
                BaseAddress = new Uri("http://localhost"),
                HandleCookies = true,
                MaxAutomaticRedirections = 3,
            }
            );
        }

      protected async Task<TokenResponse> RequestAuthToken(
          string emailAddress,
          string password
          )
        {
          var response =
            await HttpClient.RequestPasswordTokenAsync(
              new PasswordTokenRequest
            {
                Address = "https://localhost:5001/connect/token",

                ClientId = "test",
                ClientSecret = "secret",
                Scope = "api",

                UserName = emailAddress,
                Password = password,
                }
                )
            .ConfigureAwait(false);
            if (response.IsError)
            {
              throw new Exception(response.Error);
            }
            return response;
        }

      protected async Task Authorize(
          string username,
          string password
          )
        {
          var tokenResponse =
            await RequestAuthToken(
                username, password
                )
            .ConfigureAwait(false);
            HttpClient.SetBearerToken(tokenResponse.AccessToken);
        }

        protected Task<HttpResponseMessage> QueryGraphQl(
            string query,
            string? operationName = null,
            Dictionary<string, object?>? variables = null
            )
        {
            return HttpClient.PostAsync(
                "/graphql",
                MakeJsonHttpContent(
                  new GraphQlRequest(
                      query: query,
                      operationName: operationName,
                      variables: variables
                    )
                  )
                );
        }

      private HttpContent MakeJsonHttpContent<TContent>(
          TContent content
          )
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());
            var result =
              new ByteArrayContent(
                JsonSerializer.SerializeToUtf8Bytes<TContent>(
                  content,
                  options
                  )
                );
            result.Headers.ContentType =
              new MediaTypeHeaderValue("application/json");
            return result;
        }

    private sealed class GraphQlRequest
    {
        public string query { get; }
        public string? operationName { get; }
        public Dictionary<string, object?>? variables { get; }

        public GraphQlRequest(
            string query,
            string? operationName,
            Dictionary<string, object?>? variables
            )
        {
            this.query = query;
            this.operationName = operationName;
            this.variables = variables;
        }
    }
    }
}
