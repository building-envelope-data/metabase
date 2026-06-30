using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using GreenDonut.Data;
using Metabase.Data;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.DataFormats;

public sealed class DataFormatDataLoaders
: DataLoaders
{
    [DataLoader]
    public static ValueTask<IReadOnlyDictionary<Guid, DataFormat>> GetDataFormatByIdAsync(
        IReadOnlyList<Guid> ids,
        QueryContext<DataFormat> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
    {
        return GetEntityByIdAsync(
            ids,
            databaseContext => databaseContext.DataFormats,
            queryContext,
            databaseContextFactory,
            cancellationToken
        );
    }
}