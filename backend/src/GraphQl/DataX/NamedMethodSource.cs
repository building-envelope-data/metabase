namespace Metabase.GraphQl.DataX;

public sealed record NamedMethodSource(
    string Name,
    CrossDatabaseDataReference Value
);