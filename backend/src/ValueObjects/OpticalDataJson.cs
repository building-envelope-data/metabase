using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel; // ReadOnlyDictionary
using System.IO;
using System.Linq;
using CSharpFunctionalExtensions;
using Array = System.Array;
using ErrorBuilder = HotChocolate.ErrorBuilder;
using ErrorCodes = Icon.ErrorCodes;
using IError = HotChocolate.IError;
using JsonArray = Manatee.Json.JsonArray;
using JsonObject = Manatee.Json.JsonObject;
using JsonSyntaxException = Manatee.Json.JsonSyntaxException;
using JsonValue = Manatee.Json.JsonValue;
using JsonValueType = Manatee.Json.JsonValueType;

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