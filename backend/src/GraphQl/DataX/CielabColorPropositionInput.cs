namespace Metabase.GraphQl.DataX
{
    public record CielabColorPropositionInput(
        FloatPropositionInput? LStar,
        FloatPropositionInput? AStar,
        FloatPropositionInput? BStar
    );
}