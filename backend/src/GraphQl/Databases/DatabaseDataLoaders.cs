using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using GreenDonut.Data;
using Metabase.Data;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Databases;

public sealed class DatabaseDataLoaders
: DataLoaders
{
    [DataLoader]
    public static ValueTask<IReadOnlyDictionary<Guid, Database>> GetDatabaseByIdAsync(
        IReadOnlyList<Guid> ids,
        QueryContext<Database> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
    {
        return GetEntityByIdAsync(
            ids,
            databaseContext => databaseContext.Databases,
            queryContext,
            databaseContextFactory,
            cancellationToken
        );
    }
}