using System;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Data;
using Metabase.GraphQl.Institutions;

namespace Metabase.GraphQl.DataX;

public sealed class CrossDatabaseDataReference
{
    public CrossDatabaseDataReference(
        Guid dataId,
        DateTime dataTimestamp,
        DataKind dataKind,
        Guid databaseId
    )
    {
        DataId = dataId;
        DataTimestamp = dataTimestamp;
        DataKind = dataKind;
        DatabaseId = databaseId;
    }

    public Guid DataId { get; }
    public DateTime DataTimestamp { get; }
    public DataKind DataKind { get; }
    public Guid DatabaseId { get; }

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