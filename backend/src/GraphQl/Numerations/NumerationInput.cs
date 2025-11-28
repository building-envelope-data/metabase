using Metabase.Data;

namespace Metabase.GraphQl.Numerations;

public sealed record NumerationInput(
    string? Prefix,
    string MainNumber,
    string? Suffix
)
{
    public Numeration ToDomainModel()
    {
        return new(
            Prefix,
            MainNumber,
            Suffix
        );
    }
};