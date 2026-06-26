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
using OpenIddict.Abstractions;

namespace Metabase.GraphQl.OpenIdConnect.Tokens;

[ExtendObjectType(nameof(Query))]
public sealed class OpenIdConnectTokenQueries
{
    [Authorize(Policy = AuthorizationPolicies.AuthenticatedPolicy)]
    public string? GetCurrentOpenIdConnectTokenClientId(
        ClaimsPrincipal claimsPrincipal
    )
    {
        return claimsPrincipal.GetClaim(OpenIddictConstants.Claims.ClientId)
            ?? claimsPrincipal.GetClaim(OpenIddictConstants.Claims.AuthorizedParty);
    }

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
            authorization.ReportUnauthorizedError(resolverContext);
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
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToManageToken(claimsPrincipal, id, cancellationToken))
        {
            authorization.ReportUnauthorizedError(resolverContext);
            return null;
        }
        return await byId.LoadAsync(id, cancellationToken);
    }
}