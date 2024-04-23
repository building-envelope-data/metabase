using System;
using System.Threading;
using System.Threading.Tasks;
using Metabase.GraphQl.Institutions;

namespace Metabase.GraphQl.DataX;

public sealed class CrossDatabaseDataReference
{
    public Guid DataId { get; }
    public DateTime DataTimestamp { get; }
    public DataKind DataKind { get; }
    public Guid DatabaseId { get; }

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

    public Task<Metabase.Data.Institution?> GetDatabaseAsync(
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