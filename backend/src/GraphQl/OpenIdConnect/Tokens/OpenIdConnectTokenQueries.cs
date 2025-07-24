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

namespace Metabase.GraphQl.OpenIdConnect.Tokens;

[ExtendObjectType(nameof(Query))]
public sealed class OpenIdConnectTokenQueries
{
    [UseUserManager]
    [Authorize(Policy = AuthConfiguration.ReadPolicy)]
    public async Task<IAsyncEnumerable<OpenIdConnectToken>> GetOpenIdConnectTokensAsync(
        ClaimsPrincipal claimsPrincipal,
        Authorization.OpenIdConnectAuthorization authorization,
        OpenIddictTokenManager<OpenIdConnectToken> tokenManager, // TODO Make the token manager use the scoped database context.
        CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToManageApplications(claimsPrincipal, cancellationToken))
        {
            return AsyncEnumerable.Empty<OpenIdConnectToken>();
        }
        return tokenManager.ListAsync(cancellationToken: cancellationToken);
    }

    [UseUserManager]
    [Authorize(Policy = AuthConfiguration.ReadPolicy)]
    public async Task<OpenIdConnectToken?> GetOpenIdConnectTokenAsync(
        Guid uuid,
        ClaimsPrincipal claimsPrincipal,
        Authorization.OpenIdConnectAuthorization authorization,
        OpenIddictTokenManager<OpenIdConnectToken> tokenManager,
        CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToManageToken(claimsPrincipal, uuid, tokenManager, cancellationToken))
        {
            return null;
        }

        return await tokenManager.FindByIdAsync(uuid.ToString(), cancellationToken: cancellationToken);
    }
}