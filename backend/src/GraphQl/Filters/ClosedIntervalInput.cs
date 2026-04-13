namespace Metabase.GraphQl.Filters;

internal sealed record ClosedIntervalInput<T>(
    T LowerBound,
    T UpperBound
)
where T : notnull;