using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Metabase.Data.OpenIdConnect;
using Metabase.GraphQl.Users;
using OpenIddict.Core;

namespace Metabase.GraphQl.OpenIdConnect.Authorizations;

[ExtendObjectType(nameof(Query))]
public sealed class OpenIdConnectAuthorizationQueries
{
    [UseUserManager]
    [Authorize(Policy = Authorization.AuthorizationPolicies.ManageOpenIdConnectPolicy)]
    public async IAsyncEnumerable<OpenIdConnectAuthorization> GetOpenIdConnectAuthorizationsAsync(
        ClaimsPrincipal claimsPrincipal,
        Authorization.OpenIdConnectAuthorization authorization, // TODO Make the authorization manager use the scoped database context.
        OpenIddictAuthorizationManager<OpenIdConnectAuthorization> authorizationManager,
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToManageOpenIdConnect(claimsPrincipal, cancellationToken))
        {
            yield break;
        }
        await foreach (var auth in authorizationManager.ListAsync(cancellationToken: cancellationToken))
        {
            yield return auth;
        }
    }

    [UseUserManager]
    [Authorize(Policy = Authorization.AuthorizationPolicies.ManageOpenIdConnectPolicy)]
    public async Task<OpenIdConnectAuthorization?> GetOpenIdConnectAuthorization(
        Guid id,
        ClaimsPrincipal claimsPrincipal,
        Authorization.OpenIdConnectAuthorization authorization,
        OpenIddictAuthorizationManager<OpenIdConnectAuthorization> authorizationManager,
        CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToManageAuthorization(claimsPrincipal, id, authorizationManager, cancellationToken))
        {
            return null;
        }
        return await authorizationManager.FindByIdAsync(id.ToString(), cancellationToken: cancellationToken);
    }
}