using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Metabase.Data;

namespace Metabase.Json;

// Inspired by https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/converters-how-to
public sealed class ReferenceJsonConverter
: JsonConverter<Reference>
{
    public ReferenceJsonConverter()
    {
    }

    private static JsonConverter<IReference> GetReferenceConverter(JsonSerializerOptions options)
        => (JsonConverter<IReference>)options.GetConverter(typeof(IReference));

    public override Reference? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var reference = GetReferenceConverter(options)
            .Read(ref reader, typeof(IReference), options);
        return reference switch
        {
            Standard standard => new Reference(standard),
            Publication publication => new Reference(publication),
            _ => throw new JsonException()
        };
    }

    public override void Write(Utf8JsonWriter writer, Reference value, JsonSerializerOptions options)
    {
        GetReferenceConverter(options)
            .Write(writer, value.TheReference, options);
    }
}