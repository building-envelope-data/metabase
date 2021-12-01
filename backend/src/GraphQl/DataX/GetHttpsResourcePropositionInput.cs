namespace Metabase.GraphQl.DataX
{
    public record GetHttpsResourcePropositionInput(
        UuidPropositionInput? DataFormatId,
        FilesMetaInformationPropositionInput? ArchivedFilesMetaInformation
    );
}
