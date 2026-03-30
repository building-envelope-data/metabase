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
}