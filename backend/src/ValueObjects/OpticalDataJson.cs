using System;
using System.IO;
using Array = System.Array;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using ErrorBuilder = HotChocolate.ErrorBuilder;
using IError = HotChocolate.IError;
using ErrorCodes = Icon.ErrorCodes;
using System.Linq;
using System.Collections.ObjectModel; // ReadOnlyDictionary
using JsonValue = Manatee.Json.JsonValue;
using JsonObject = Manatee.Json.JsonObject;
using JsonArray = Manatee.Json.JsonArray;
using JsonValueType = Manatee.Json.JsonValueType;
using JsonSyntaxException = Manatee.Json.JsonSyntaxException;
using System.Collections.Immutable;

namespace Icon.ValueObjects
{
    public sealed class OpticalDataJson
      : ValueObject
    {
        private static readonly IImmutableSet<string> JsonSchemaUris
          = JsonSchema.FilterUris(@"^.*/optical\.json$").ToImmutableHashSet();

        public Json Json { get; }

        private OpticalDataJson(Json json)
        {
            Json = json;
        }

        public static Result<OpticalDataJson, Errors> From(
            object? nestedCollectionsJson,
            IReadOnlyList<object>? path = null
            )
        {
            return
              Json.From(nestedCollectionsJson, path)
              .Bind(json =>
                  json.ExtractSchemaUri().Bind(schemaUri =>
                    JsonSchemaUris.Contains(schemaUri)
                    ? JsonSchema.Get(schemaUri)
                      .Bind(jsonSchema =>
                        jsonSchema.Validate(json, path)
                        .Map(validatedJson =>
                          new OpticalDataJson(validatedJson)
                          )
                        )
                    : Result.Failure<OpticalDataJson, Errors>(
                        Errors.One(
                          message: $"The JSON schema URI `{schemaUri}` for optical data is none of the supported ones: `{string.Join("`, `", JsonSchemaUris)}`",
                          code: ErrorCodes.InvalidValue
                          )
                        )
                    )
                  );
        }

        public object? ToNestedCollections()
        {
            return Json.ToNestedCollections();
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Json;
        }

        public static implicit operator Json(OpticalDataJson opticalDataJson)
        {
            return opticalDataJson.Json;
        }
    }
}
