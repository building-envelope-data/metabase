namespace Metabase.GraphQl.DataX
{
    public sealed record CielabColorPropositionInput(
        FloatPropositionInput? LStar,
        FloatPropositionInput? AStar,
        FloatPropositionInput? BStar
    );
}