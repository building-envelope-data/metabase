using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Metabase.Configuration;
using Metabase.Data.OpenIdConnect;
using Metabase.GraphQl.Users;
using OpenIddict.Core;
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace Metabase.GraphQl.OpenIdConnect.Authorizations;

[ExtendObjectType(nameof(Query))]
public sealed class OpenIdConnectAuthorizationQueries
{
    [UseUserManager]
    [Authorize(Policy = AuthConfiguration.ReadPolicy)]
    public async IAsyncEnumerable<OpenIdConnectAuthorization> GetOpenIdConnectAuthorizationsAsync(
        ClaimsPrincipal claimsPrincipal,
        Authorization.OpenIdConnectAuthorization authorization, // TODO Make the authorization manager use the scoped database context.
        OpenIddictAuthorizationManager<OpenIdConnectAuthorization> authorizationManager,
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToManage(claimsPrincipal, cancellationToken))
        {
            yield break;
        }
        await foreach (var auth in authorizationManager.ListAsync(cancellationToken: cancellationToken))
        {
            yield return auth;
        }
    }

    [UseUserManager]
    [Authorize(Policy = AuthConfiguration.ReadPolicy)]
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