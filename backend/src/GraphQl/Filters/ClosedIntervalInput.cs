namespace Metabase.GraphQl.Filters;

internal record ClosedIntervalInput<T>(
    T LowerBound,
    T UpperBound
)
where T : notnull;