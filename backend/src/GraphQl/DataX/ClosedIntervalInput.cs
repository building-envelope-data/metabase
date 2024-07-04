namespace Metabase.GraphQl.DataX;

public sealed record ClosedIntervalInput(
    double LowerBound,
    double UpperBound
);