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
    public class TestBase : IClassFixture<CustomWebApplicationFactory>
    {
        protected static async Task<TokenResponse> RequestAuthToken(HttpClient httpClient, string emailAddress, string password)
        {
            var response = await httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = "https://localhost:5001/connect/token",

                ClientId = "test",
                ClientSecret = "secret",
                Scope = "api",

                UserName = emailAddress,
                Password = password,
            });
            if (response.IsError) throw new Exception(response.Error); // TODO Is this exception propagated?
            return response;
        }

        protected static async Task Authorize(HttpClient httpClient, string username, string password)
        {
            var tokenResponse = await RequestAuthToken(httpClient, username, password);
            httpClient.SetBearerToken(tokenResponse.AccessToken);
        }

        protected CustomWebApplicationFactory Factory { get; }
        protected HttpClient HttpClient { get; }

        protected TestBase(CustomWebApplicationFactory factory)
        {
            Factory = factory;
            Factory.SetUp();
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
    }
}