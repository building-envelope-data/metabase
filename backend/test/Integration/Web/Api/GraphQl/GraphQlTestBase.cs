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

namespace Test.Integration.Web.Api.GraphQl
{
    public abstract class GraphQlTestBase : TestBase
    {
        protected GraphQlClient Client { get; }

        protected GraphQlTestBase(CustomWebApplicationFactory factory)
          : base(factory)
        {
            Client = new GraphQlClient(HttpClient);
        }
    }
}