using System;
using System.Text;

namespace Metabase.Extensions;

public static class StringExtensions
{
    public static string FirstCharToLower(this string value)
    {
        return string.IsNullOrEmpty(value)
               || !char.IsLetter(value, 0)
               || char.IsLower(value, 0)
            ? value
            : char.ToLowerInvariant(value[0]) + value[1..];
    }

    public static string? NullIfEmpty(this string value)
        => string.IsNullOrEmpty(value) ? null : value;

    public static string? NullIfWhitespace(this string value)
        => string.IsNullOrWhiteSpace(value) ? null : value;

    public static string Base64Encode(this string plainText)
    {
        return Convert.ToBase64String(
            Encoding.UTF8.GetBytes(plainText)
        );
    }

    public static string Base64Decode(this string base64EncodedData)
    {
        return Encoding.UTF8.GetString(
            Convert.FromBase64String(base64EncodedData)
        );
    }

    public static string Enquote(this string str)
    {
        return "\"" + str + "\"";
    }
}