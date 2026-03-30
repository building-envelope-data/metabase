using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Data;
using Metabase.GraphQl.DataFormats;

namespace Metabase.GraphQl.DataX;

public sealed record GetHttpsResource(
    string? Description,
    string HashValue,
    Uri Locator,
    Guid DataFormatId,
    IReadOnlyList<FileMetaInformation> ArchivedFilesMetaInformation
)
{
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