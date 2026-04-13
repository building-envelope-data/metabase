using System;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;

namespace Metabase.Json;

// Inspired by https://learn.microsoft.com/en-us/dotnet/standard/datetime/system-text-json-support#use-datetimeoffsetparse-as-a-fallback
public sealed class OffsetDateTimeConverterUsingDateTimeParseAsFallback : JsonConverter<OffsetDateTime>
{
    public override OffsetDateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var converter = NodaConverters.OffsetDateTimeConverter;
        Debug.Assert(converter.CanConvert(typeToConvert));
        try
        {
            // OffsetDateTimePattern.ExtendedIso.Parse(reader.GetString());
            return converter.Read(ref reader, typeToConvert, options);
        }
        catch (JsonException)
        {
            return OffsetDateTime.FromDateTimeOffset(
                DateTimeOffset.Parse(reader.GetString() ?? "",
                CultureInfo.InvariantCulture)
            );
        }
    }

    public override void Write(Utf8JsonWriter writer, OffsetDateTime value, JsonSerializerOptions options)
    {
        // For information on the format specifier `o`, see
        // https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings#the-round-trip-o-o-format-specifier
        writer.WriteStringValue(value.ToString("o", CultureInfo.InvariantCulture));
    }
}