using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Metabase.Configuration;
using Metabase.Data.OpenIdConnect;
using Metabase.GraphQl.Users;
using OpenIddict.Core;

namespace Metabase.GraphQl.OpenIdConnect.Authorizations;

[ExtendObjectType(nameof(Query))]
public sealed class OpenIdConnectAuthorizationQueries
{
    [UseUserManager]
    [Authorize(Policy = AuthConfiguration.ReadPolicy)]
    public async Task<IAsyncEnumerable<OpenIdConnectAuthorization>> GetOpenIdConnectAuthorizationsAsync(
        ClaimsPrincipal claimsPrincipal,
        Authorization.OpenIdConnectAuthorization authorization, // TODO Make the authorization manager use the scoped database context.
        OpenIddictAuthorizationManager<OpenIdConnectAuthorization> authorizationManager,
        CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToManageApplications(claimsPrincipal, cancellationToken))
        {
            return AsyncEnumerable.Empty<OpenIdConnectAuthorization>();
        }
        return authorizationManager.ListAsync(cancellationToken: cancellationToken);
    }

    [UseUserManager]
    [Authorize(Policy = AuthConfiguration.ReadPolicy)]
    public async Task<OpenIdConnectAuthorization?> GetOpenIdConnectAuthorization(
        Guid uuid,
        ClaimsPrincipal claimsPrincipal,
        Authorization.OpenIdConnectAuthorization authorization,
        OpenIddictAuthorizationManager<OpenIdConnectAuthorization> authorizationManager,
        CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToManageAuthorization(claimsPrincipal, uuid, authorizationManager, cancellationToken))
        {
            return null;
        }
        return await authorizationManager.FindByIdAsync(uuid.ToString(), cancellationToken: cancellationToken);
    }
}