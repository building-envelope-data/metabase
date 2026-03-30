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

namespace Metabase.GraphQl.OpenIdConnect.Applications;

[ExtendObjectType(nameof(Query))]
public sealed class OpenIdConnectApplicationQueries
{
    [UseUserManager]
    [Authorize(Policy = AuthorizationPolicies.ManageOpenIdConnectScopePolicy)]
    public Task<OpenIdConnectApplication?> GetCurrentOpenIdConnectApplicationAsync(
        ClaimsPrincipal claimsPrincipal,
        Authorization.OpenIdConnectAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.SwitchUserOrApplicationAsync(
            claimsPrincipal,
            user => Task.FromResult<OpenIdConnectApplication?>(null),
            async application =>
            {
                if (application is not null
                    && !await authorization.IsAuthorizedToManageApplication(claimsPrincipal, application.Id, cancellationToken)
                )
                {
                    return null;
                }
                return application;
            },
            cancellationToken
        );
    }

    // TODO In all queries, instead of returning nothing, report as authentication error to client.
    [UsePaging]
    [UseFiltering<OpenIdConnectApplicationFilterType>]
    [UseSorting<OpenIdConnectApplicationSortType>]
    [UseUserManager]
    [Authorize(Policy = AuthorizationPolicies.ManageOpenIdConnectScopePolicy)]
    public async ValueTask<HotChocolate.Types.Pagination.Connection<OpenIdConnectApplication>> GetOpenIdConnectApplicationsAsync(
        IResolverContext resolverContext,
        ApplicationDbContext databaseContext,
        ClaimsPrincipal claimsPrincipal,
        Authorization.OpenIdConnectAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToManageOpenIdConnect(claimsPrincipal, cancellationToken))
        {
            return HotChocolate.Types.Pagination.Connection.Empty<OpenIdConnectApplication>();
        }
        return await databaseContext.OpenIdConnectApplications
            .AsNoTracking()
            .With(resolverContext.GetQueryContext<OpenIdConnectApplication>(), Sorting.DefaultEntityOrder)
            .ToPageAsync(resolverContext.GetPagingArguments(), cancellationToken)
            .ToConnectionAsync();
    }

    [UseUserManager]
    [Authorize(Policy = AuthorizationPolicies.ManageOpenIdConnectScopePolicy)]
    public async Task<OpenIdConnectApplication?> GetOpenIdConnectApplicationAsync(
        Guid id,
        IOpenIdConnectApplicationByIdDataLoader byId,
        ClaimsPrincipal claimsPrincipal,
        Authorization.OpenIdConnectAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToManageApplication(claimsPrincipal, id, cancellationToken))
        {
            return null;
        }
        return await byId.LoadAsync(id, cancellationToken);
    }
}