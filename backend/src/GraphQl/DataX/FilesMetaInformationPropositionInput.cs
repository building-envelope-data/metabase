namespace Metabase.GraphQl.DataX
{
    public sealed record FilesMetaInformationPropositionInput(
        FileMetaInformationPropositionInput? All,
        FileMetaInformationPropositionInput? None,
        FileMetaInformationPropositionInput? Some
    );
}