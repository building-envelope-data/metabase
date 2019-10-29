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

namespace Test.Integration.Web.Api.Controller
{
    public abstract class ClientBase
    {
        protected static HttpContent MakeJsonHttpContent<TContent>(TContent content)
        {
            var result = new ByteArrayContent(JsonSerializer.SerializeToUtf8Bytes<TContent>(content));
            result.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return result;
        }

        protected HttpClient HttpClient { get; }

        public ClientBase(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }
    }
}