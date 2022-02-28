namespace Metabase.GraphQl.DataX
{
    public record CielabColorsPropositionInput(
        CielabColorPropositionInput? All,
        CielabColorPropositionInput? None,
        CielabColorPropositionInput? Some
    );
}