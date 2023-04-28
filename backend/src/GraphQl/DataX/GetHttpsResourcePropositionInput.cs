namespace Metabase.GraphQl.DataX
{
    public sealed record GetHttpsResourcePropositionInput(
        UuidPropositionInput? DataFormatId,
        FilesMetaInformationPropositionInput? ArchivedFilesMetaInformation
    );
}
