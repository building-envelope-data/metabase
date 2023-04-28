namespace Metabase.GraphQl.DataX
{
    public sealed record CielabColorsPropositionInput(
        CielabColorPropositionInput? All,
        CielabColorPropositionInput? None,
        CielabColorPropositionInput? Some
    );
}