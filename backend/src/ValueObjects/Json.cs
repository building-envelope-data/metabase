using System;
using System.Collections.Generic;
using System.Collections.ObjectModel; // ReadOnlyDictionary
using System.IO;
using System.Linq;
using CSharpFunctionalExtensions;
using Array = System.Array;
using ErrorBuilder = HotChocolate.ErrorBuilder;
using ErrorCodes = Icon.ErrorCodes;
using IError = HotChocolate.IError;
using JArray = Newtonsoft.Json.Linq.JArray;
using JObject = Newtonsoft.Json.Linq.JObject;
using JsonArray = Manatee.Json.JsonArray;
using JsonElement = System.Text.Json.JsonElement;
using JsonObject = Manatee.Json.JsonObject;
using JsonSyntaxException = Manatee.Json.JsonSyntaxException;
using JsonValue = Manatee.Json.JsonValue;
using JsonValueKind = System.Text.Json.JsonValueKind;
using JsonValueType = Manatee.Json.JsonValueType;

namespace Icon.ValueObjects
{
    public sealed class Json
      : ValueObject
    {
        private const string SchemaKey = "schema";

        /* private static object ConvertJTokenToNestedCollections(object jsonToken) */
        /* { */
        /*   // If we switch to `System.Text.Json.JsonElement`, we can use */
        /*   // `JsonSerializer.Deserialize<Dictionary<string, object>>` */
        /*   if (jsonToken is JObject jsonObject) */
        /*     return */
        /*       new ReadOnlyDictionary<string, object>( */
        /*           jsonObject */
        /*           .ToObject<IDictionary<string, object>>() */
        /*           .ToDictionary( */
        /*             pair => pair.Key, */
        /*             pair => ConvertJTokenToNestedCollections(pair.Value) */
        /*             ) */
        /*           ); */
        /*   if (jsonToken is JArray jsonArray) */
        /*     return */
        /*       jsonArray */
        /*       .ToObject<IList<object>>() */
        /*       .Select(ConvertJTokenToNestedCollections) */
        /*       .ToList() */
        /*       .AsReadOnly(); */
        /*   if (jsonToken is JValue jsonValue) */
        /*     return jsonValue.Value; */
        /*   throw new Exception($"The JSON token {jsonToken} has an un-supported type."); */
        /* } */

        private static object? ConvertJsonValueToNestedCollections(JsonValue jsonValue)
        {
            return jsonValue.Type switch
            {
                JsonValueType.Array =>
                  jsonValue.Array
                  .Select(ConvertJsonValueToNestedCollections)
                  .ToList().AsReadOnly(),
                JsonValueType.Boolean => jsonValue.Boolean,
                JsonValueType.Null => null,
                JsonValueType.Number => jsonValue.Number,
                JsonValueType.Object =>
                  new ReadOnlyDictionary<string, object?>(
                      jsonValue.Object.ToDictionary(
                        pair => pair.Key,
                        pair => ConvertJsonValueToNestedCollections(pair.Value)
                        )
                      ),
                JsonValueType.String => jsonValue.String,
                // God-damned C# does not have switch expression exhaustiveness for
                // enums as mentioned for example on https://github.com/dotnet/csharplang/issues/2266
                _ => throw new Exception($"The JSON value `{jsonValue}` of type `{jsonValue.Type}` fell through")
            };
        }

        private static Result<JsonValue, Errors> ConvertNestedCollectionsToJsonValue(
            object? jsonValue,
            IReadOnlyList<object>? path = null
            )
        {
            return jsonValue switch
            {
                IList<object?> list =>
                  list.Select((v, index) =>
                      ConvertNestedCollectionsToJsonValue(
                        v,
                        path?.Append(index).ToList().AsReadOnly()
                        )
                      )
                  .Combine()
                  .Map(jsonValues =>
                      new JsonValue(
                        new JsonArray(jsonValues)
                        )
                      ),
                bool boolean => Result.Ok<JsonValue, Errors>(new JsonValue(boolean)),
                null => Result.Ok<JsonValue, Errors>(JsonValue.Null),
                // For the list of implicit conversions to `double` see
                // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/numeric-conversions#implicit-numeric-conversions
                // and for the explicit ones see
                // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/numeric-conversions#explicit-numeric-conversions
                sbyte number => Result.Ok<JsonValue, Errors>(new JsonValue((double)number)),
                byte number => Result.Ok<JsonValue, Errors>(new JsonValue((double)number)),
                short number => Result.Ok<JsonValue, Errors>(new JsonValue((double)number)),
                ushort number => Result.Ok<JsonValue, Errors>(new JsonValue((double)number)),
                int number => Result.Ok<JsonValue, Errors>(new JsonValue((double)number)),
                uint number => Result.Ok<JsonValue, Errors>(new JsonValue((double)number)),
                long number => Result.Ok<JsonValue, Errors>(new JsonValue((double)number)),
                ulong number => Result.Ok<JsonValue, Errors>(new JsonValue((double)number)),
                float number => Result.Ok<JsonValue, Errors>(new JsonValue((double)number)),
                double number => Result.Ok<JsonValue, Errors>(new JsonValue(number)),
                decimal number => Result.Ok<JsonValue, Errors>(new JsonValue((double)number)),
                IDictionary<string, object?> dictionary =>
                  dictionary.Select(pair =>
                      ConvertNestedCollectionsToJsonValue(
                        pair.Value,
                        path?.Append(pair.Key).ToList().AsReadOnly()
                        )
                      .Map(jsonValue =>
                        new KeyValuePair<string, JsonValue>(pair.Key, jsonValue)
                        )
                      )
                  .Combine()
                  .Map(stringJsonValuePairs =>
                      new JsonValue(
                        new JsonObject(
                          stringJsonValuePairs.ToDictionary(
                            pair => pair.Key,
                            pair => (JsonValue?)pair.Value
                            )
                          )
                        )
                      ),
                string @string => Result.Ok<JsonValue, Errors>(new JsonValue(@string)),
                // God-damned C# does not have switch expression exhaustiveness for
                // enums as mentioned for example on https://github.com/dotnet/csharplang/issues/2266
                _ => Result.Failure<JsonValue, Errors>(
                    Errors.One(
                      message: $"The JSON value `{jsonValue}` of type `{jsonValue.GetType()}` fell through",
                      code: ErrorCodes.InvalidValue,
                      path: path
                      )
                    )
            };
        }

        private static Result<JsonValue, Errors> ConvertJsonElementToJsonValue(
            JsonElement jsonElement,
            IReadOnlyList<object>? path = null
            )
        {
            return jsonElement.ValueKind switch
            {
                JsonValueKind.Array =>
                  jsonElement.EnumerateArray()
                  .Select((v, index) =>
                      ConvertJsonElementToJsonValue(
                        v,
                        path?.Append(index).ToList().AsReadOnly()
                        )
                      )
                  .Combine()
                  .Map(jsonValues =>
                      new JsonValue(
                        new JsonArray(jsonValues)
                        )
                      ),
                JsonValueKind.False => Result.Ok<JsonValue, Errors>(new JsonValue(false)),
                JsonValueKind.Null => Result.Ok<JsonValue, Errors>(JsonValue.Null),
                JsonValueKind.Number => Result.Ok<JsonValue, Errors>(new JsonValue(jsonElement.GetDouble())),
                JsonValueKind.Object =>
                  jsonElement.EnumerateObject()
                  .Select(jsonProperty =>
                      ConvertJsonElementToJsonValue(
                        jsonProperty.Value,
                        path?.Append(jsonProperty.Name).ToList().AsReadOnly()
                        )
                      .Map(jsonValue =>
                        new KeyValuePair<string, JsonValue>(jsonProperty.Name, jsonValue)
                        )
                      )
                  .Combine()
                  .Map(stringJsonValuePairs =>
                      new JsonValue(
                        new JsonObject(
                          stringJsonValuePairs.ToDictionary(
                            pair => pair.Key,
                            pair => (JsonValue?)pair.Value
                            )
                          )
                        )
                      ),
                JsonValueKind.String => Result.Ok<JsonValue, Errors>(new JsonValue(jsonElement.GetString())),
                JsonValueKind.True => Result.Ok<JsonValue, Errors>(new JsonValue(true)),
                JsonValueKind.Undefined => Result.Failure<JsonValue, Errors>(
                    Errors.One(
                      message: $"The JSON element `{jsonElement}` is of kind `{JsonValueKind.Undefined}`",
                      code: ErrorCodes.InvalidValue,
                      path: path
                      )
                    ),
                // God-damned C# does not have switch expression exhaustiveness for
                // enums as mentioned for example on https://github.com/dotnet/csharplang/issues/2266
                _ => Result.Failure<JsonValue, Errors>(
                    Errors.One(
                      message: $"The JSON element `{jsonElement}` of kind `{jsonElement.ValueKind}` fell through",
                      code: ErrorCodes.InvalidValue,
                      path: path
                      )
                    )
            };
        }

        // We should __not__ expose the underlying `JsonValue` instance because
        // it is mutable.
        public JsonValue Value { get; }

        private Json(JsonValue value)
        {
            Value = value;
        }

        public static Result<Json, Errors> FromNestedCollections(
            object? nestedCollectionsJson,
            IReadOnlyList<object>? path = null
            )
        {
            return
              ConvertNestedCollectionsToJsonValue(nestedCollectionsJson, path)
              .Map(jsonValue => new Json(jsonValue));
        }

        public static Result<Json, Errors> FromJsonElement(
            JsonElement jsonElement,
            IReadOnlyList<object>? path = null
            )
        {
            return
              ConvertJsonElementToJsonValue(jsonElement, path)
              .Map(jsonValue => new Json(jsonValue));
        }

        public static Result<Json, Errors> FromFile(
            string filePath,
            IReadOnlyList<object>? path = null
            )
        {
            // TODO Catch other exceptions mentioned on
            // https://docs.microsoft.com/en-us/dotnet/api/system.io.file.open?view=netcore-3.1#System_IO_File_Open_System_String_System_IO_FileMode_System_IO_FileAccess_System_IO_FileShare_
            // https://gregsdennis.github.io/Manatee.Json/api/Manatee.Json.JsonValue.html#Manatee_Json_JsonValue_Parse_System_IO_TextReader_
            try
            {
                using (var fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (var streamReader = new StreamReader(fileStream))
                    {
                        return new Json(
                            JsonValue.Parse(streamReader)
                            );
                    }
                }
            }
            catch (FileNotFoundException exception)
            {
                return Result.Failure<Json, Errors>(
                    Errors.One(
                      message: $"There is no file with path {filePath} causing the exception {exception}",
                      code: ErrorCodes.FileNotFound,
                      path: path
                      )
                    );
            }
            catch (JsonSyntaxException exception)
            {
                // https://gregsdennis.github.io/Manatee.Json/usage/getting-started.html#handling-errors
                return Result.Failure<Json, Errors>(
                    Errors.One(
                      message: $"The contents of the file with path {filePath} is not valid JSON: {exception.Message}",
                      code: ErrorCodes.InvalidSyntax,
                      path: path?.Concat(exception.Location).ToList().AsReadOnly()
                      )
                    );
            }
        }

        // https://json-schema.org/draft/2019-09/json-schema-core.html#rfc.section.8.1.1
        public Result<string, Errors> ExtractSchemaUri()
        {
            if (Value.Type == JsonValueType.Object)
            {
                var rootObject = Value.Object;
                if (rootObject.ContainsKey(SchemaKey))
                {
                    if (rootObject[SchemaKey].Type == JsonValueType.String)
                    {
                        return Result.Ok<string, Errors>(
                            rootObject[SchemaKey].String
                            );
                    }
                    return Result.Failure<string, Errors>(
                        Errors.One(
                          message: $"The key `{SchemaKey}` does not have a string value",
                          code: ErrorCodes.InvalidValue
                          )
                        );
                }
                return Result.Failure<string, Errors>(
                    Errors.One(
                      message: $"The key `{SchemaKey}` does not exist",
                      code: ErrorCodes.InvalidValue
                      )
                    );
            }
            return Result.Failure<string, Errors>(
                Errors.One(
                  message: $"The JSON root element is not an object",
                  code: ErrorCodes.InvalidValue
                  )
                );
        }

        public object? ToNestedCollections()
        {
            return ConvertJsonValueToNestedCollections(Value);
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        // We should __not__ expose the underlying `JsonValue` instance because
        // it is mutable.
        public static implicit operator JsonValue(Json json)
        {
            return json.Value;
        }
    }
}