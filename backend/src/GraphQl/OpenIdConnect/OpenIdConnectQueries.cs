using System.Collections.Generic;
using System.Threading;
using HotChocolate;
using HotChocolate.Types;
using OpenIddict.Core;
using OpenIddict.EntityFrameworkCore.Models;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate.AspNetCore.Authorization;

namespace Metabase.GraphQl.OpenIdConnect
{
    [ExtendObjectType(nameof(Query))]
    public sealed class OpendIdConnectQueries
    {
        // [UseDbContext(typeof(Data.ApplicationDbContext))]
        // [UsePaging]
        // [UseProjection]
        // [UseFiltering]
        // [UseSorting]
        [Authorize(Roles = new[] { Data.Role.Administrator })]
        public async Task<List<OpenIddictEntityFrameworkCoreApplication>> GetOpenIdConnectApplications(
            [Service] OpenIddictApplicationManager<OpenIddictEntityFrameworkCoreApplication> manager,
            // [ScopedService] Data.ApplicationDbContext context, // TODO Make the application manager use the scoped database context.
            CancellationToken cancellationToken
            )
        {
            // TODO Is there a more efficient way to return an `AsyncEnumerable` or `AsyncEnumerator` or to turn such a thing into an `Enumerable` or `Enumerator`?
            return await manager.ListAsync(cancellationToken: cancellationToken).ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        [Authorize(Roles = new[] { Data.Role.Administrator })]
        public async Task<List<OpenIddictEntityFrameworkCoreScope>> GetOpenIdConnectScopes(
            [Service] OpenIddictScopeManager<OpenIddictEntityFrameworkCoreScope> manager,
            // [ScopedService] Data.ApplicationDbContext context // TODO Make the application manager use the scoped database context.
            CancellationToken cancellationToken
            )
        {
            return await manager.ListAsync(cancellationToken: cancellationToken).ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        [Authorize(Roles = new[] { Data.Role.Administrator })]
        public async Task<List<OpenIddictEntityFrameworkCoreToken>> GetOpenIdConnectTokens(
            [Service] OpenIddictTokenManager<OpenIddictEntityFrameworkCoreToken> manager,
            // [TokendService] Data.ApplicationDbContext context // TODO Make the application manager use the scoped database context.
            CancellationToken cancellationToken
            )
        {
            return await manager.ListAsync(cancellationToken: cancellationToken).ToListAsync(cancellationToken).ConfigureAwait(false);
        }

        [Authorize(Roles = new[] { Data.Role.Administrator })]
        public async Task<List<OpenIddictEntityFrameworkCoreAuthorization>> GetOpenIdConnectAuthorizations(
            [Service] OpenIddictAuthorizationManager<OpenIddictEntityFrameworkCoreAuthorization> manager,
            // [AuthorizationdService] Data.ApplicationDbContext context // TODO Make the application manager use the scoped database context.
            CancellationToken cancellationToken
            )
        {
            return await manager.ListAsync(cancellationToken: cancellationToken).ToListAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}