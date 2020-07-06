using System.Collections.Generic;
using System.Collections.Immutable;
using CSharpFunctionalExtensions;
using Errors = Infrastructure.Errors;

namespace Metabase.ValueObjects
{
    public sealed class CalorimetricDataJson
      : DataJson
    {
        private static readonly IImmutableSet<string> JsonSchemaUris
          = JsonSchema.FilterUris(@"^.*/calorimetric\.json$").ToImmutableHashSet();

        private CalorimetricDataJson(Json json)
          : base(json)
        {
        }

        public static Result<CalorimetricDataJson, Errors> FromNestedCollections(
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

        public static Result<CalorimetricDataJson, Errors> FromJsonElement(
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

        private static Result<CalorimetricDataJson, Errors> FromJson(
            Json json,
            IReadOnlyList<object>? path = null
            )
        {
            return
              FromJson(
                  json,
                  JsonSchemaUris,
                  validatedJson => new CalorimetricDataJson(validatedJson),
                  path
                  );
        }
    }
}