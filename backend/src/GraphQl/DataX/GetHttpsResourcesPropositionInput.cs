namespace Metabase.GraphQl.DataX
{
    public record GetHttpsResourcesPropositionInput(
        GetHttpsResourcePropositionInput? All,
        GetHttpsResourcePropositionInput? None,
        GetHttpsResourcePropositionInput? Some
    );
}
