using System;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Data;
using Metabase.GraphQl.DataFormats;

namespace Metabase.GraphQl.DataX;

public sealed class GetHttpsResource
{
    // public IReadOnlyList<FileMetaInformation> ArchivedFilesMetaInformation { get; }

    public GetHttpsResource(
        string description,
        string hashValue,
        Uri locator,
        Guid dataFormatId
    )
    {
        Description = description;
        HashValue = hashValue;
        Locator = locator;
        DataFormatId = dataFormatId;
    }

    public string Description { get; }
    public string HashValue { get; }
    public Uri Locator { get; }
    public Guid DataFormatId { get; }

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