namespace Metabase.GraphQl.DataX
{
    public sealed record FloatsPropositionInput(
        FloatPropositionInput? All,
        FloatPropositionInput? None,
        FloatPropositionInput? Some
        );
}
