using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Metabase.Authorization;
using Metabase.Configuration;
using Metabase.Data;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Core;

namespace Metabase.GraphQl.OpenIdConnect.Scopes;

[ExtendObjectType(nameof(Query))]
public sealed class ScopeQueries
{
    [UseUserManager]
    [Authorize(Policy = AuthConfiguration.ReadPolicy)]
    public async Task<IAsyncEnumerable<OpenIdScope>> GetScopes(
        OpenIddictScopeManager<OpenIdScope> manager,
        ClaimsPrincipal claimsPrincipal,
        UserManager<User> userManager,
        ApplicationDbContext context, // TODO Make the scope manager use the scoped database context.
        CancellationToken cancellationToken
    )
    {
        if (!await OpenIdConnectAuthorization.IsAuthorizedToViewApplications(claimsPrincipal, userManager, context, cancellationToken)
                .ConfigureAwait(false))
        {
            return AsyncEnumerable.Empty<OpenIdScope>();
        }
        return manager.ListAsync(cancellationToken: cancellationToken);
    }

    [UseUserManager]
    [Authorize(Policy = AuthConfiguration.ReadPolicy)]
    public async Task<OpenIdScope?> GetScope(
        Guid scopeId,
        OpenIddictScopeManager<OpenIdScope> manager,
        ClaimsPrincipal claimsPrincipal,
        UserManager<User> userManager,
        ApplicationDbContext context, // TODO Make the scope manager use the scoped database context.
        CancellationToken cancellationToken
    )
    {
        if (!await OpenIdConnectAuthorization.IsAuthorizedToViewApplications(claimsPrincipal, userManager, context, cancellationToken)
                .ConfigureAwait(false))
        {
            return null;
        }

        return await manager.FindByIdAsync(scopeId.ToString(), cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}