using System;
using Array = System.Array;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using ErrorBuilder = HotChocolate.ErrorBuilder;
using IError = HotChocolate.IError;
using ErrorCodes = Icon.ErrorCodes;
using System.Linq;
using System.Collections.ObjectModel; // ReadOnlyDictionary

namespace Icon.ValueObjects
{
    public sealed class JsonSchema
      : ValueObject
    {
        public static readonly IReadOnlyDictionary<string, JsonSchema> JsonSchemaRegistry
          = CreateJsonSchemaRegistryFromFiles();

        public static Result<JsonSchema, Errors> Get(
            string schemaUri,
            IReadOnlyList<object>? path = null
            )
        {
            if (JsonSchemaRegistry.ContainsKey(schemaUri))
            {
              return JsonSchemaRegistry[schemaUri];
            }
            return Result.Failure<JsonSchema, Errors>(
                Errors.One(
                  message: $"There is no JSON schema with URI {schemaUri}",
                  code: ErrorCodes.InvalidValue,
                  path: path
                  )
                );
        }

        public static IEnumerable<string> FilterUris(string regularExpression)
        {
            return JsonSchemaRegistry.Keys
              .Where(schemaUri =>
                  System.Text.RegularExpressions.Regex.IsMatch(schemaUri, regularExpression)
                  );
        }

        private const string JsonSchemasDirectoryPath = "./json-schemas/";
        private const string JsonSchemaFileNamePattern = "*.json";
        // For some reason `JsonSchemasDirectoryEnumerationOptions` is `null` when used within `LoadJsonSchemaFiles`. Why?
        /* private static readonly System.IO.EnumerationOptions JsonSchemasDirectoryEnumerationOptions = */
        /*   new System.IO.EnumerationOptions */
        /*   { */
        /*     AttributesToSkip = System.IO.FileAttributes.Hidden | System.IO.FileAttributes.System, */
        /*     BufferSize = 0, */
        /*     IgnoreInaccessible = true, */
        /*     MatchCasing = System.IO.MatchCasing.PlatformDefault, */
        /*     MatchType = System.IO.MatchType.Simple, */
        /*     RecurseSubdirectories = true, */
        /*     ReturnSpecialDirectories = false */
        /*   }; */

        private static Result<IEnumerable<JsonSchema>, Errors> LoadJsonSchemaFiles()
        {
            var jsonSchemasDirectoryEnumerationOptions = new System.IO.EnumerationOptions
            {
              AttributesToSkip = System.IO.FileAttributes.Hidden | System.IO.FileAttributes.System,
              BufferSize = 0,
              IgnoreInaccessible = true,
              MatchCasing = System.IO.MatchCasing.PlatformDefault,
              MatchType = System.IO.MatchType.Simple,
              RecurseSubdirectories = true,
              ReturnSpecialDirectories = false
            };
            return
              System.IO.Directory.GetFiles(
                JsonSchemasDirectoryPath,
                JsonSchemaFileNamePattern,
                jsonSchemasDirectoryEnumerationOptions
                )
              .Select(filePath =>
                  JsonSchema.FromFile(filePath)
                  )
              .Combine();
        }

        private static IReadOnlyDictionary<string, JsonSchema> CreateJsonSchemaRegistryFromFiles()
        {
            var result =
              LoadJsonSchemaFiles()
              .Bind(jsonSchemas =>
                jsonSchemas.Select(jsonSchema =>
                    jsonSchema.Id()
                    )
                  .Combine()
                  .Map(ids =>
                    {
                    var jsonSchemaRegistry = new Dictionary<string, JsonSchema>();
                    foreach (var (id, jsonSchema) in ids.Zip(jsonSchemas))
                    {
                    jsonSchemaRegistry[id] = jsonSchema;
                    // https://gregsdennis.github.io/Manatee.Json/usage/schema/references.html
                    Manatee.Json.Schema.JsonSchemaRegistry.Register(jsonSchema.Value);
                    }
                    return new ReadOnlyDictionary<string, JsonSchema>(jsonSchemaRegistry);
                    }
                    )
                );
            if (result.IsFailure)
            {
                throw new Exception($"Registering the JSON schema files failed with the following error: {result.Error}");
            }
            return result.Value;
        }

        private static readonly Manatee.Json.Schema.JsonSchemaOptions _validationOptions =
          new Manatee.Json.Schema.JsonSchemaOptions(Manatee.Json.Schema.JsonSchemaOptions.Default)
          {
            AllowUnknownFormats = false,
            OutputFormat = Manatee.Json.Schema.SchemaValidationOutputFormat.Basic,
            ValidateFormatKeyword = true
          };

        private static Result<JsonSchema, Errors> From(
            Manatee.Json.Schema.JsonSchema jsonSchema,
            IReadOnlyList<object>? path = null
            )
        {
          var validationResults = jsonSchema.ValidateSchema(); // https://gregsdennis.github.io/Manatee.Json/api/Manatee.Json.Schema.MetaSchemaValidationResults.html
          if (validationResults.IsValid)
          {
            return Result.Ok<JsonSchema, Errors>(
                new JsonSchema(jsonSchema)
                );
          }
          return Result.Failure<JsonSchema, Errors>(
              Errors.One(
                message: $"The given JSON schema is in-valid: {validationResults}`",
                code: ErrorCodes.InvalidValue,
                path: path
                )
              );
        }

        private static Result<JsonSchema, Errors> From(
            Json json,
            IReadOnlyList<object>? path = null
            )
        {
            try
            {
              var jsonSchema =
                new Manatee.Json.Serialization.JsonSerializer()
                .Deserialize<Manatee.Json.Schema.JsonSchema>(json);
              return From(jsonSchema);
            }
            catch (Manatee.Json.Serialization.TypeDoesNotContainPropertyException exception)
            {
              return Result.Failure<JsonSchema, Errors>(
                  Errors.One(
                    message: $"The given JSON contains a property, namely, `{exception.Json}`, which is not defined by the type `{exception.Type}`",
                    code: ErrorCodes.InvalidValue,
                    path: path
                    )
                  );
            }
        }

        private static Result<JsonSchema, Errors> FromFile(
            string filePath,
            IReadOnlyList<object>? path = null
            )
        {
            return
              Json.FromFile(filePath, path)
              .Bind(json => From(json, path));
        }

        // Do __not__ expose the underlying `Manatee.Json.Schema.JsonSchema`
        // instance because it is mutable.
        private Manatee.Json.Schema.JsonSchema Value { get; }

        private JsonSchema(Manatee.Json.Schema.JsonSchema value)
        {
            Value = value;
        }

        public Result<string, Errors> Id()
        {
            if (Value.Id is null)
            {
              return Result.Failure<string, Errors>(
                  Errors.One(
                    message: $"The JSON schema {this} does not have an `$id`",
                    code: ErrorCodes.InvalidValue
                    )
                  );
            }
            return Result.Ok<string, Errors>(Value.Id);
        }

        public Result<Json, Errors> Validate(
            Json json,
            IReadOnlyList<object>? path = null
            )
        {
            var validationResults = Value.Validate(json, _validationOptions);
            if (validationResults.IsValid)
            {
                return Result.Ok<Json, Errors>(json);
            }
            return Result.Failure<Json, Errors>(
                Errors.From(
                  new List<Manatee.Json.Schema.SchemaValidationResults> { validationResults }
                  .Concat(validationResults.NestedResults)
                  .Select(validationResult =>
                    Errors.OneX(
                      message: $"The JSON does not conform to the schema: The error is {validationResult.ErrorMessage}, the absolute location is {validationResult.AbsoluteLocation}, and the keyword is {validationResult.Keyword}",
                      code: ErrorCodes.InvalidValue,
                      path: path?.Concat(validationResult.InstanceLocation).ToList().AsReadOnly()
                      )

                    )
                  )
                );
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        // Do __not__ expose the underlying `Manatee.Json.Schema.JsonSchema`
        // instance because it is mutable.
        // public static implicit operator Manatee.Json.Schema.JsonSchema(JsonSchema jsonSchema)
        // {
        //     return jsonSchema.Value;
        // }
    }
}
