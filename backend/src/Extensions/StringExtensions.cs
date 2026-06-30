using System;
using System.Text;
using System.Diagnostics.Contracts;

namespace Metabase.Extensions;

public static class StringExtensions
{
    [Pure]
    public static string FirstCharToLower(this string value)
    {
        return string.IsNullOrEmpty(value)
               || !char.IsLetter(value, 0)
               || char.IsLower(value, 0)
            ? value
            : char.ToLowerInvariant(value[0]) + value[1..];
    }

    [Pure]
    public static string? NullIfEmpty(this string value)
        => string.IsNullOrEmpty(value) ? null : value;

    [Pure]
    public static string? NullIfWhitespace(this string value)
        => string.IsNullOrWhiteSpace(value) ? null : value;

    [Pure]
    public static string Base64Encode(this string plainText)
    {
        return Convert.ToBase64String(
            Encoding.UTF8.GetBytes(plainText)
        );
    }

    [Pure]
    public static string Base64Decode(this string base64EncodedData)
    {
        return Encoding.UTF8.GetString(
            Convert.FromBase64String(base64EncodedData)
        );
    }

    [Pure]
    public static string Enquote(this string str)
    {
        return "\"" + str + "\"";
    }
}