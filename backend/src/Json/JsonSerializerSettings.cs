using System.Text.Json;
using System.Text.Json.Serialization;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;

namespace Metabase.Json;

/// <summary>
/// Options for JSON serialization.
/// </summary>
public static class JsonSerializerSettings
{
    private static readonly JsonSerializerOptions s_common =
        new JsonSerializerOptions()
        {
            Converters =
            {
                new OffsetDateTimeConverterUsingDateTimeParseAsFallback()
            },
            AllowTrailingCommas = true,
            AllowOutOfOrderMetadataProperties = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            IgnoreReadOnlyFields = true,
            IgnoreReadOnlyProperties = false,
            IncludeFields = false,
            NumberHandling = JsonNumberHandling.Strict,
            PreferredObjectCreationHandling = JsonObjectCreationHandling.Replace,
            PropertyNameCaseInsensitive = false,
            ReadCommentHandling = JsonCommentHandling.Disallow,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            RespectNullableAnnotations = true,
            UnknownTypeHandling = JsonUnknownTypeHandling.JsonElement,
            UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
            WriteIndented = false,
            AllowDuplicateProperties = false,
        }
        .ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);

    public static readonly JsonSerializerOptions GraphQl =
        new(s_common)
        {
            // because `Converters` is read-only, the converters below are `Add`ed according to https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/object-and-collection-initializers#object-initializers-with-collection-read-only-property-initialization
            Converters =
            {
                new JsonStringEnumConverter(new ToConstantCaseJsonNamingPolicy(), allowIntegerValues: false)
            },
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        }; //.SetupImmutableConverter();

    public static readonly JsonSerializerOptions Rest =
        new(s_common)
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        };

    public static readonly JsonSerializerOptions BedJson =
        new(s_common)
        {
            // because `Converters` is read-only, the converters below are `Add`ed according to https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/object-and-collection-initializers#object-initializers-with-collection-read-only-property-initialization
            Converters = {
                new JsonStringEnumConverter(new ToCamelCaseJsonNamingPolicy(), allowIntegerValues: false)
            },
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip,
        };

    public static readonly JsonSerializerOptions Compact =
        new(s_common)
        {
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };
}