namespace Metabase.GraphQl.DataX
{
    public sealed record GetHttpsResourcesPropositionInput(
        GetHttpsResourcePropositionInput? All,
        GetHttpsResourcePropositionInput? None,
        GetHttpsResourcePropositionInput? Some
    );
}