using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Data;
using Metabase.GraphQl.DataFormats;

namespace Metabase.GraphQl.DataX;

public sealed class GetHttpsResource
{
    private const string BedJsonGuid = "9ca9e8f5-94bf-4fdd-81e3-31a58d7ca708";
    private const string LbnlKlemsGuid = "e021cf20-e887-4dce-ad27-35da70cec472";

    internal static GetHttpsResource From(GetHttpsResourceIgsdb resource)
    {
        return new GetHttpsResource(
            "",
            "",
            resource.Locator,
            GuessDataFormatId(resource.Locator),
            Array.Empty<FileMetaInformation>().AsReadOnly()
        );
    }

    private static Guid GuessDataFormatId(Uri locator)
    {
        if (locator.Query.Contains("bed-json"))
        {
            return new Guid(BedJsonGuid);
        }
        return new Guid(LbnlKlemsGuid);
    }

    public GetHttpsResource(
        string description,
        string hashValue,
        Uri locator,
        Guid dataFormatId,
        IReadOnlyList<FileMetaInformation> archivedFilesMetaInformation
    )
    {
        Description = description;
        HashValue = hashValue;
        Locator = locator;
        DataFormatId = dataFormatId;
        ArchivedFilesMetaInformation = archivedFilesMetaInformation;
    }

    public string Description { get; }
    public string HashValue { get; }
    public Uri Locator { get; }
    public Guid DataFormatId { get; }
    public IReadOnlyList<FileMetaInformation> ArchivedFilesMetaInformation { get; }

    public Task<DataFormat?> GetDataFormatAsync(
        DataFormatByIdDataLoader dataFormatById,
        CancellationToken cancellationToken
    )
    {
        return dataFormatById.LoadAsync(
            DataFormatId,
            cancellationToken
        );
    }
}