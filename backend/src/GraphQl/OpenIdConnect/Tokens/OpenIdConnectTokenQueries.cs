using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut.Data;
using HotChocolate.Authorization;
using HotChocolate.Data;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.Data.OpenIdConnect;
using Metabase.GraphQl.Extensions;
using Metabase.GraphQl.Users;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.OpenIdConnect.Tokens;

[ExtendObjectType(nameof(Query))]
public sealed class OpenIdConnectTokenQueries
{
    // TODO In all queries, instead of returning nothing, report as authentication error to client.
    [UsePaging]
    [UseFiltering<OpenIdConnectTokenFilterType>]
    [UseSorting<OpenIdConnectTokenSortType>]
    [UseUserManager]
    [Authorize(Policy = AuthorizationPolicies.ManageOpenIdConnectScopePolicy)]
    public async ValueTask<HotChocolate.Types.Pagination.Connection<OpenIdConnectToken>> GetOpenIdConnectTokensAsync(
        IResolverContext resolverContext,
        ApplicationDbContext databaseContext,
        ClaimsPrincipal claimsPrincipal,
        Authorization.OpenIdConnectAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToManageOpenIdConnect(claimsPrincipal, cancellationToken))
        {
            return HotChocolate.Types.Pagination.Connection.Empty<OpenIdConnectToken>();
        }
        return await databaseContext.OpenIdConnectTokens
            .AsNoTracking()
            .With(resolverContext.GetQueryContext<OpenIdConnectToken>(), Sorting.DefaultEntityOrder)
            .ToPageAsync(resolverContext.GetPagingArguments(), cancellationToken)
            .ToConnectionAsync();
    }

    [UseUserManager]
    [Authorize(Policy = AuthorizationPolicies.ManageOpenIdConnectScopePolicy)]
    public async Task<OpenIdConnectToken?> GetOpenIdConnectTokenAsync(
        Guid id,
        IOpenIdConnectTokenByIdDataLoader byId,
        ClaimsPrincipal claimsPrincipal,
        Authorization.OpenIdConnectAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToManageToken(claimsPrincipal, id, cancellationToken))
        {
            return null;
        }
        return await byId.LoadAsync(id, cancellationToken);
    }
}