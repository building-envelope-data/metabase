using Startup = Icon.Startup;
using System.Collections.Generic;
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

    public abstract class OutputBase
    {
        // https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to#handle-overflow-json
        [JsonExtensionData]
        public Dictionary<string, JsonElement> ExtensionData { get; set; }
    }

    public abstract class ClientBase<TOutput> : ClientBase
    {
        public static T EnsureEmptyExtensionData<T>(T output) where T : OutputBase
        {
            if (output.ExtensionData != null && output.ExtensionData.Count != 0)
            {
                throw new JsonException($"The extension data dictionary is not empty but contains the key(s) '{String.Join("', '", output.ExtensionData.Keys)}'");
            }
            return output;
        }

        public static IEnumerable<T> EnsureEmptyExtensionData<T>(IEnumerable<T> outputs) where T : OutputBase
        {
            foreach (var output in outputs)
            {
                EnsureEmptyExtensionData(output);
            }
            return outputs;
        }

        protected JsonSerializerOptions JsonSerializerOptions { get; }

        public ClientBase(HttpClient httpClient) : base(httpClient)
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

        public async Task<TOutput> Deserialize(HttpResponseMessage httpResponse)
        {
            httpResponse.EnsureSuccessStatusCode();
            return await JsonSerializer.DeserializeAsync<TOutput>(
                await httpResponse.Content.ReadAsStreamAsync(),
                JsonSerializerOptions
            );
        }
    }
}