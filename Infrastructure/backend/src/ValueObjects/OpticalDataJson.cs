using System.Collections.Generic;
using System.Collections.Immutable;
using CSharpFunctionalExtensions;
using Errors = Infrastructure.Errors;

namespace Infrastructure.ValueObjects
{
    public sealed class OpticalDataJson
      : DataJson
    {
        private static readonly IImmutableSet<string> JsonSchemaUris
          = JsonSchema.FilterUris(@"^.*/optical\.json$").ToImmutableHashSet();

        private OpticalDataJson(Json json)
          : base(json)
        {
        }

        public static Result<OpticalDataJson, Errors> FromNestedCollections(
            object? nestedCollectionsJson,
            IReadOnlyList<object>? path = null
            )
        {
            return
              Json.FromNestedCollections(nestedCollectionsJson, path)
              .Bind(json =>
                  FromJson(json, path)
                  );
        }

        public static Result<OpticalDataJson, Errors> FromJsonElement(
            System.Text.Json.JsonElement jsonElement,
            IReadOnlyList<object>? path = null
            )
        {
            return
              Json.FromJsonElement(jsonElement, path)
              .Bind(json =>
                  FromJson(json, path)
                  );
        }

        private static Result<OpticalDataJson, Errors> FromJson(
            Json json,
            IReadOnlyList<object>? path = null
            )
        {
            return
              FromJson(
                  json,
                  JsonSchemaUris,
                  validatedJson => new OpticalDataJson(validatedJson),
                  path
                  );
        }
    }
}