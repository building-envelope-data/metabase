using System.Collections.Generic;
using System.Text.Json;
using JsonExtensionDataAttribute = System.Text.Json.Serialization.JsonExtensionDataAttribute;

namespace Test.Integration.Web.Api.GraphQl
{
    public abstract class ResponseBase
    {
        public static IEnumerable<T> EnsureNoOverflow<T>(
            IEnumerable<T> responses
            )
          where T : ResponseBase
        {
            foreach (var response in responses)
            {
                response.EnsureNoOverflow();
            }
            return responses;
        }

        // https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to#handle-overflow-json
        [JsonExtensionDataAttribute]
        public Dictionary<string, JsonElement>? OverflowData { get; set; }

        public virtual void EnsureNoOverflow()
        {
            if (OverflowData != null && OverflowData.Count != 0)
            {
                throw new JsonException($"The extension data dictionary is not empty but contains the key(s) '{string.Join("', '", OverflowData.Keys)}'");
            }
        }
    }
}