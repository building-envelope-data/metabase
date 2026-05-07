using System;
using System.Buffers;
using System.Text.Unicode;
using GreenDonut.Data.Cursors.Serializers;
using Metabase.Extensions;
using NodaTime;
using NodaTime.Text;

namespace Metabase.GraphQl.NodaTime;

// Inspired by https://github.com/ChilliCream/graphql-platform/blob/main/src/GreenDonut/src/GreenDonut.Data/Cursors/Serializers/DateTimeOffsetCursorKeySerializer.cs
public sealed class OffsetDateTimeCursorKeySerializer : ICursorKeySerializer
{
    private static readonly OffsetDateTimePattern _pattern = OffsetDateTimePattern.ExtendedIso;

    public bool IsSupported(Type type)
        => type == typeof(OffsetDateTime) || type == typeof(OffsetDateTime?);

    public System.Reflection.MethodInfo GetCompareToMethod(Type type)
        => typeof(OffsetDateTime).GetMethod(nameof(NodaTimeExtensions.CompareTo), [typeof(OffsetDateTime), typeof(OffsetDateTime)])!;

    public object Parse(ReadOnlySpan<byte> formattedKey)
    {
        Span<char> chars = stackalloc char[formattedKey.Length];
        if (Utf8.ToUtf16(formattedKey, chars, out _, out _) != OperationStatus.Done)
        {
            throw new FormatException("Invalid cursor format");
        }
        var result = _pattern.Parse(new string(chars));
        if (!result.Success) throw new FormatException("Could not parse OffsetDateTime cursor");
        return result.Value;
    }

    public bool TryFormat(object key, Span<byte> buffer, out int written)
    {
        var value = (OffsetDateTime)key;
        var formatted = _pattern.Format(value);
        return Utf8.FromUtf16(formatted, buffer, out _, out written) == OperationStatus.Done;
    }
}