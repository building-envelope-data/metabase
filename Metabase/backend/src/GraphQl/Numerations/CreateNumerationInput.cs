namespace Metabase.GraphQl.Numerations
{
    public record CreateNumerationInput(
            string? Prefix,
            string MainNumber,
            string? Suffix
    );
}