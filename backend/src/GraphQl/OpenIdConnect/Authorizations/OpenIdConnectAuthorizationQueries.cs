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
using Metabase.Data;
using Metabase.Data.OpenIdConnect;
using Metabase.GraphQl.Extensions;
using Metabase.GraphQl.Users;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.OpenIdConnect.Authorizations;

[ExtendObjectType(nameof(Query))]
public sealed class OpenIdConnectAuthorizationQueries
{
    // TODO In all queries, instead of returning nothing, report as authentication error to client.
    [UsePaging]
    [UseFiltering<OpenIdConnectAuthorizationFilterType>]
    [UseSorting<OpenIdConnectAuthorizationSortType>]
    [UseUserManager]
    [Authorize(Policy = Authorization.AuthorizationPolicies.ManageOpenIdConnectScopePolicy)]
    public async ValueTask<HotChocolate.Types.Pagination.Connection<OpenIdConnectAuthorization>> GetOpenIdConnectAuthorizationsAsync(
        IResolverContext resolverContext,
        ApplicationDbContext databaseContext,
        ClaimsPrincipal claimsPrincipal,
        Authorization.OpenIdConnectAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToManageOpenIdConnect(claimsPrincipal, cancellationToken))
        {
            return HotChocolate.Types.Pagination.Connection.Empty<OpenIdConnectAuthorization>();
        }
        return await databaseContext.OpenIdConnectAuthorizations
            .AsNoTracking()
            .With(resolverContext.GetQueryContext<OpenIdConnectAuthorization>(), Sorting.DefaultEntityOrder)
            .ToPageAsync(resolverContext.GetPagingArguments(), cancellationToken)
            .ToConnectionAsync();
    }

    [UseUserManager]
    [Authorize(Policy = Authorization.AuthorizationPolicies.ManageOpenIdConnectScopePolicy)]
    public async Task<OpenIdConnectAuthorization?> GetOpenIdConnectAuthorizationAsync(
        Guid id,
        IOpenIdConnectAuthorizationByIdDataLoader byId,
        ClaimsPrincipal claimsPrincipal,
        Authorization.OpenIdConnectAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToManageAuthorization(claimsPrincipal, id, cancellationToken))
        {
            return null;
        }
        return await byId.LoadAsync(id, cancellationToken);
    }
}