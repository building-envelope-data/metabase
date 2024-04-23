namespace Metabase.GraphQl.DataX
{
    public sealed record FloatPropositionInput(
        double? EqualTo,
        double? GreaterThanOrEqualTo,
        ClosedIntervalInput? InClosedInterval,
        double? LessThanOrEqualTo
    );
}