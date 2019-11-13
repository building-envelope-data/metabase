using System.Collections.Generic;
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
using JsonExtensionDataAttribute = System.Text.Json.Serialization.JsonExtensionDataAttribute;

namespace Test.Integration.Web.Api.GraphQl
{
    public class Response<TData>
      : ResponseBase
      where TData : ResponseBase
    {
        public TData data { get; set; }
        public IEnumerable<Dictionary<string, object>> errors { get; set; }

        public void EnsureNoData()
        {
            if (data != null)
            {
                throw new JsonException($"The data value is not empty but contains '{JsonSerializer.Serialize(data)}'");
            }
        }

        public void EnsureNoErrors()
        {
            if (errors != null)
            {
                throw new JsonException($"The errors dictionary is not empty but contains '{JsonSerializer.Serialize(errors)}'");
            }
        }
    }
}