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
    public abstract class DataJson
      : ValueObject
    {
        public Json Json { get; }

        protected DataJson(Json json)
        {
            Json = json;
        }

        protected static Result<TDataJson, Errors> FromJson<TDataJson>(
            Json json,
            IImmutableSet<string> jsonSchemaUris,
            Func<Json, TDataJson> newDataJson,
            IReadOnlyList<object>? path = null
            )
        {
            return
              json.ExtractSchemaUri().Bind(schemaUri =>
                jsonSchemaUris.Contains(schemaUri)
                ? JsonSchema.Get(schemaUri)
                  .Bind(jsonSchema =>
                    jsonSchema.Validate(json, path)
                    .Map(validatedJson =>
                      newDataJson(validatedJson)
                      )
                    )
                : Result.Failure<TDataJson, Errors>(
                    Errors.One(
                      message: $"The JSON schema URI `{schemaUri}` for optical data is none of the supported ones: `{string.Join("`, `", jsonSchemaUris)}`",
                      code: ErrorCodes.InvalidValue
                      )
                    )
                );
        }

        public object ToNestedCollections()
        {
            // In `From` we make sure that the JSON ist a JSON Object with at
            // least the key `schema`. So, using the null-forgiving operator
            // `!` is safe here.
            return Json.ToNestedCollections()!;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Json;
        }

        public static implicit operator Json(DataJson opticalDataJson)
        {
            return opticalDataJson.Json;
        }
    }
}
