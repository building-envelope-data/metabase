using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Metabase.Authorization;
using Metabase.Data.OpenIdConnect;
using Metabase.GraphQl.Users;
using OpenIddict.Core;

namespace Metabase.GraphQl.OpenIdConnect.Tokens;

[ExtendObjectType(nameof(Query))]
public sealed class OpenIdConnectTokenQueries
{
    [UseUserManager]
    [Authorize(Policy = AuthorizationPolicies.ManageOpenIdConnectPolicy)]
    public async IAsyncEnumerable<OpenIdConnectToken> GetOpenIdConnectTokensAsync(
        ClaimsPrincipal claimsPrincipal,
        Authorization.OpenIdConnectAuthorization authorization,
        OpenIddictTokenManager<OpenIdConnectToken> tokenManager, // TODO Make the token manager use the scoped database context.
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToManageOpenIdConnect(claimsPrincipal, cancellationToken))
        {
            yield break;
        }
        await foreach (var token in tokenManager.ListAsync(cancellationToken: cancellationToken))
        {
            yield return token;
        }
    }

    [UseUserManager]
    [Authorize(Policy = AuthorizationPolicies.ManageOpenIdConnectPolicy)]
    public async Task<OpenIdConnectToken?> GetOpenIdConnectTokenAsync(
        Guid id,
        ClaimsPrincipal claimsPrincipal,
        Authorization.OpenIdConnectAuthorization authorization,
        OpenIddictTokenManager<OpenIdConnectToken> tokenManager,
        CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToManageToken(claimsPrincipal, id, tokenManager, cancellationToken))
        {
            return null;
        }

        return await tokenManager.FindByIdAsync(id.ToString(), cancellationToken: cancellationToken);
    }
}