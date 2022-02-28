namespace Metabase.GraphQl.DataX
{
    public record FilesMetaInformationPropositionInput(
        FileMetaInformationPropositionInput? All,
        FileMetaInformationPropositionInput? None,
        FileMetaInformationPropositionInput? Some
    );
}
