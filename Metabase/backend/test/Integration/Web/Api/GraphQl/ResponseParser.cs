using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

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
                await httpResponse.Content.ReadAsStreamAsync().ConfigureAwait(false),
                JsonSerializerOptions
            ).ConfigureAwait(false);
            response.EnsureNoOverflow();
            return response;
        }
    }
}