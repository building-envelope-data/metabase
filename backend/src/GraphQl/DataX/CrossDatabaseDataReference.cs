using System;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Data;
using Metabase.GraphQl.Databases;
using NodaTime;

namespace Metabase.GraphQl.DataX;

public sealed record CrossDatabaseDataReference(
    Guid DataId,
    OffsetDateTime DataTimestamp,
    DataKind DataKind,
    Guid DatabaseId
)
{
    public Task<Database?> GetDatabaseAsync(
        DatabaseByIdDataLoader databaseById,
        CancellationToken cancellationToken
    )
    {
        return databaseById.LoadAsync(
            DatabaseId,
            cancellationToken
        );
    }
}