using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using IdentityModel.Client;
using Xunit;
using Configuration = Icon.Configuration;
using GrantType = IdentityServer4.Models.GrantType;
using JsonExtensionDataAttribute = System.Text.Json.Serialization.JsonExtensionDataAttribute;
using Startup = Icon.Startup;
using TokenRequest = IdentityModel.Client.TokenRequest;
using TokenResponse = IdentityModel.Client.TokenResponse;
using WebApplicationFactoryClientOptions = Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions;

namespace Test.Integration.Web.Api.GraphQl
{
    public class ResponseParser
    {
        protected JsonSerializerOptions JsonSerializerOptions { get; }

        public ResponseParser()
        {
            // https://docs.microsoft.com/en-us/dotnet/api/system.text.json.jsonserializeroptions?view=netcore-3.0
            // https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to
            JsonSerializerOptions = new JsonSerializerOptions
            {
                AllowTrailingCommas = false,
                // DefaultBufferSize = ...,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                // Encoder = ...,
                IgnoreNullValues = false,
                IgnoreReadOnlyProperties = true,
                // MaxDepth
                PropertyNameCaseInsensitive = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                ReadCommentHandling = JsonCommentHandling.Disallow,
                WriteIndented = false,
            };
        }

        public async Task<Response<TData, TError>> Parse<TData, TError>(HttpResponseMessage httpResponse)
          where TData : ResponseBase
          where TError : ResponseBase
        {
            httpResponse.EnsureSuccessStatusCode();
            var response =
              await JsonSerializer.DeserializeAsync<Response<TData, TError>>(
                await httpResponse.Content.ReadAsStreamAsync(),
                JsonSerializerOptions
            );
            response.EnsureNoOverflow();
            return response;
        }
    }
}