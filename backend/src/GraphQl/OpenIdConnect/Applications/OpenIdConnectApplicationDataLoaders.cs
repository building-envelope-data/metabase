using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using GreenDonut.Data;
using Metabase.Data;
using Metabase.Data.OpenIdConnect;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

public sealed class OpenIdConnectApplicationDataLoaders
: DataLoaders
{
    [DataLoader]
    public static ValueTask<IReadOnlyDictionary<Guid, OpenIdConnectApplication>> GetOpenIdConnectApplicationByIdAsync(
        IReadOnlyList<Guid> ids,
        QueryContext<OpenIdConnectApplication> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
    {
        return GetEntityByIdAsync(
            ids,
            databaseContext => databaseContext.OpenIdConnectApplications,
            queryContext,
            databaseContextFactory,
            cancellationToken
        );
    }

    [DataLoader]
    public static async ValueTask<IReadOnlyDictionary<string, OpenIdConnectApplication>> GetOpenIdConnectApplicationByClientIdAsync(
        IReadOnlyList<string> clientIds,
        QueryContext<OpenIdConnectApplication> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
    {
        await using var databaseContext = await databaseContextFactory.CreateDbContextAsync(cancellationToken);
        return await databaseContext.OpenIdConnectApplications
            .AsNoTrackingWithIdentityResolution()
            .Where(_ => clientIds.Contains(_.ClientId ?? ""))
            .With(queryContext, Sorting.DefaultEntityOrder)
            .ToDictionaryAsync(_ => _.ClientId ?? "", cancellationToken);
    }
}