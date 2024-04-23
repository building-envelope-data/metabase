namespace Metabase.Extensions
{
    public static class StringExtensions
    {
        public static string FirstCharToLower(this string str)
        {
            return string.IsNullOrEmpty(str)
                   || !char.IsLetter(str, 0)
                   || char.IsLower(str, 0)
                ? str
                : char.ToLowerInvariant(str[0]) + str[1..];
        }
    }
}