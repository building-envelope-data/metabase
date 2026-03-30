using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using GreenDonut.Data;
using Metabase.Data;
using Metabase.Data.OpenIdConnect;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.OpenIdConnect.Authorizations;

public sealed class OpenIdConnectAuthorizationDataLoaders
: DataLoaders
{
    [DataLoader]
    public static ValueTask<IReadOnlyDictionary<Guid, OpenIdConnectAuthorization>> GetOpenIdConnectAuthorizationByIdAsync(
        IReadOnlyList<Guid> ids,
        QueryContext<OpenIdConnectAuthorization> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
    {
        return GetEntityByIdAsync(
            ids,
            databaseContext => databaseContext.OpenIdConnectAuthorizations,
            queryContext,
            databaseContextFactory,
            cancellationToken
        );
    }
}