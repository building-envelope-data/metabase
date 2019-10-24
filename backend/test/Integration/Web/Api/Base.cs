using Startup = Icon.Startup;
using GrantType = IdentityServer4.Models.GrantType;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Xunit;
using WebApplicationFactoryClientOptions = Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions;
using Configuration = Icon.Configuration;
using TokenResponse = IdentityModel.Client.TokenResponse;
using TokenRequest = IdentityModel.Client.TokenRequest;
using IdentityModel.Client;

namespace Test.Integration.Web.Api
{
    public class Base : IClassFixture<CustomWebApplicationFactory>
    {
        protected CustomWebApplicationFactory Factory { get; }
        protected HttpClient HttpClient { get; }

        protected Base(CustomWebApplicationFactory factory)
        {
            Factory = factory.SetUp();
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

        protected async Task<TokenResponse> RequestAuthToken(string emailAddress, string password)
        {
            var response = await HttpClient.RequestTokenAsync(new TokenRequest
            {
                Address = "https://localhost:5001/connect/authorize",
                GrantType = GrantType.Implicit,

                ClientId = "swagger",
                ClientSecret = Configuration.Auth.ApiSecret,

                Parameters =
                  {
                  {"scope", Configuration.Auth.ApiName},
                  {"redirect_uri", "https://localhost:5001/swagger/oauth2-redirect.html"},
                  {"response_type", "token"},
                  {"response_mode", "fragment"},
                  {"realm", "icon"}
                  /* {"email", emailAddress}, */
                  /* {"password", password} */
                  }
            });
            if (response.IsError) throw new Exception(response.Error);
            return response;
        }

        protected HttpContent MakeJsonHttpContent<TContent>(TContent content)
        {
            var result = new ByteArrayContent(JsonSerializer.SerializeToUtf8Bytes<TContent>(content));
            result.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return result;
        }
    }
}