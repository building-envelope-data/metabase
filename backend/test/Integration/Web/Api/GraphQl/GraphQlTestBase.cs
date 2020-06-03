using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using IdentityModel.Client;
using Xunit;
using Configuration = Icon.Configuration;
using GrantType = IdentityServer4.Models.GrantType;
using Startup = Icon.Startup;
using TokenRequest = IdentityModel.Client.TokenRequest;
using TokenResponse = IdentityModel.Client.TokenResponse;
using WebApplicationFactoryClientOptions = Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions;

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