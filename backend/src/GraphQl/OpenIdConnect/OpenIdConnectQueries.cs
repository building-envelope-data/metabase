using System.Collections.Generic;
using System.Threading;
using HotChocolate;
using HotChocolate.Types;
using OpenIddict.Core;
using OpenIddict.EntityFrameworkCore.Models;
using System.Linq;
using System.Threading.Tasks;
using Metabase.Authorization;
using HotChocolate.Data;
using Metabase.GraphQl.Users;
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Metabase.GraphQl.OpenIdConnect
{
    [ExtendObjectType(nameof(Query))]
    public sealed class OpendIdConnectQueries
    {
        // TODO In all queries, instead of returning nothing, report as authentication error to client.
        [UseUserManager]
        public async Task<IList<OpenIddictEntityFrameworkCoreApplication>> GetOpenIdConnectApplications(
            [Service] OpenIddictApplicationManager<OpenIddictEntityFrameworkCoreApplication> manager,
            ClaimsPrincipal claimsPrincipal,
            [Service(ServiceKind.Resolver)] UserManager<Data.User> userManager,
            // Data.ApplicationDbContext context, // TODO Make the application manager use the scoped database context.
            CancellationToken cancellationToken
        )
        {
            if (!await OpenIdConnectAuthorization.IsAuthorizedToView(claimsPrincipal, userManager)
                    .ConfigureAwait(false))
            {
                return Array.Empty<OpenIddictEntityFrameworkCoreApplication>();
            }

            // TODO Is there a more efficient way to return an `AsyncEnumerable` or `AsyncEnumerator` or to turn such a thing into an `Enumerable` or `Enumerator`?
            return await manager.ListAsync(cancellationToken: cancellationToken).ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        [UseUserManager]
        public async Task<IList<OpenIddictEntityFrameworkCoreScope>> GetOpenIdConnectScopes(
            [Service] OpenIddictScopeManager<OpenIddictEntityFrameworkCoreScope> manager,
            ClaimsPrincipal claimsPrincipal,
            [Service(ServiceKind.Resolver)] UserManager<Data.User> userManager,
            // Data.ApplicationDbContext context // TODO Make the application manager use the scoped database context.
            CancellationToken cancellationToken
        )
        {
            if (!await OpenIdConnectAuthorization.IsAuthorizedToView(claimsPrincipal, userManager)
                    .ConfigureAwait(false))
            {
                return Array.Empty<OpenIddictEntityFrameworkCoreScope>();
            }

            return await manager.ListAsync(cancellationToken: cancellationToken).ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        [UseUserManager]
        public async Task<IList<OpenIddictEntityFrameworkCoreToken>> GetOpenIdConnectTokens(
            [Service] OpenIddictTokenManager<OpenIddictEntityFrameworkCoreToken> manager,
            ClaimsPrincipal claimsPrincipal,
            [Service(ServiceKind.Resolver)] UserManager<Data.User> userManager,
            // [TokendService] Data.ApplicationDbContext context // TODO Make the application manager use the scoped database context.
            CancellationToken cancellationToken
        )
        {
            if (!await OpenIdConnectAuthorization.IsAuthorizedToView(claimsPrincipal, userManager)
                    .ConfigureAwait(false))
            {
                return Array.Empty<OpenIddictEntityFrameworkCoreToken>();
            }

            return await manager.ListAsync(cancellationToken: cancellationToken).ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        [UseUserManager]
        public async Task<IList<OpenIddictEntityFrameworkCoreAuthorization>> GetOpenIdConnectAuthorizations(
            [Service] OpenIddictAuthorizationManager<OpenIddictEntityFrameworkCoreAuthorization> manager,
            ClaimsPrincipal claimsPrincipal,
            [Service(ServiceKind.Resolver)] UserManager<Data.User> userManager,
            // [AuthorizationdService] Data.ApplicationDbContext context // TODO Make the application manager use the scoped database context.
            CancellationToken cancellationToken
        )
        {
            if (!await OpenIdConnectAuthorization.IsAuthorizedToView(claimsPrincipal, userManager)
                    .ConfigureAwait(false))
            {
                return Array.Empty<OpenIddictEntityFrameworkCoreAuthorization>();
            }

            return await manager.ListAsync(cancellationToken: cancellationToken).ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }
    }
}