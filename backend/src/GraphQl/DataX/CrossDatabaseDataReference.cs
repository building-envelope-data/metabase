using System;
using System.Threading;
using System.Threading.Tasks;
using NodaTime;
using Metabase.Data;
using Metabase.GraphQl.Institutions;

namespace Metabase.GraphQl.DataX;

public sealed class CrossDatabaseDataReference(
    Guid dataId,
    OffsetDateTime dataTimestamp,
    DataKind dataKind,
    Guid databaseId
    )
{
    public Guid DataId { get; } = dataId;
    public OffsetDateTime DataTimestamp { get; } = dataTimestamp;
    public DataKind DataKind { get; } = dataKind;
    public Guid DatabaseId { get; } = databaseId;

    public Task<Institution?> GetDatabaseAsync(
        InstitutionByIdDataLoader databaseById,
        CancellationToken cancellationToken
    )
    {
        return databaseById.LoadAsync(
            DatabaseId,
            cancellationToken
        );
    }
}