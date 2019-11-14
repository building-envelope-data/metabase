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