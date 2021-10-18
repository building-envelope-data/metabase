namespace Metabase.GraphQl.DataX
{
    public record FloatsPropositionInput(
        FloatPropositionInput? All,
        FloatPropositionInput? None,
        FloatPropositionInput? Some
        );
}
