using System.Collections.Generic;
using System.Collections.Immutable;
using CSharpFunctionalExtensions;
using Errors = Infrastructure.Errors;

namespace Metabase.ValueObjects
{
    public sealed class HygrothermalDataJson
      : DataJson
    {
        private static readonly IImmutableSet<string> JsonSchemaUris
          = JsonSchema.FilterUris(@"^.*/hygrothermal\.json$").ToImmutableHashSet();

        private HygrothermalDataJson(Json json)
          : base(json)
        {
        }

        public static Result<HygrothermalDataJson, Errors> FromNestedCollections(
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

        public static Result<HygrothermalDataJson, Errors> FromJsonElement(
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

        private static Result<HygrothermalDataJson, Errors> FromJson(
            Json json,
            IReadOnlyList<object>? path = null
            )
        {
            return
              FromJson(
                  json,
                  JsonSchemaUris,
                  validatedJson => new HygrothermalDataJson(validatedJson),
                  path
                  );
        }
    }
}