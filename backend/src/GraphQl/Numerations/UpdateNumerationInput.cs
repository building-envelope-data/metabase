namespace Metabase.GraphQl.Numerations
{
    public record UpdateNumerationInput(
            string? Prefix,
            string MainNumber,
            string? Suffix
    );
}