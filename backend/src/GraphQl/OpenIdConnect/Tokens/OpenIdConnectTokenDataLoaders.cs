using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using GreenDonut.Data;
using Metabase.Data;
using Metabase.Data.OpenIdConnect;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.OpenIdConnect.Tokens;

public sealed class OpenIdConnectTokenDataLoaders
: DataLoaders
{
    [DataLoader]
    public static ValueTask<IReadOnlyDictionary<Guid, OpenIdConnectToken>> GetOpenIdConnectTokenByIdAsync(
        IReadOnlyList<Guid> ids,
        QueryContext<OpenIdConnectToken> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
    {
        return GetEntityByIdAsync(
            ids,
            databaseContext => databaseContext.OpenIdConnectTokens,
            queryContext,
            databaseContextFactory,
            cancellationToken
        );
    }
}