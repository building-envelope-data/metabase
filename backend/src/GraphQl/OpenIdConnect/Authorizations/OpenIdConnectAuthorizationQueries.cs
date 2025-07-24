using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Metabase.Configuration;
using Metabase.Data;
using Metabase.Data.OpenIdConnect;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Core;

namespace Metabase.GraphQl.OpenIdConnect.Authorizations;

[ExtendObjectType(nameof(Query))]
public sealed class OpenIdConnectAuthorizationQueries
{
    [UseUserManager]
    [Authorize(Policy = AuthConfiguration.ReadPolicy)]
    public async Task<IAsyncEnumerable<OpenIdConnectAuthorization>> GetOpenIdConnectAuthorizationsAsync(
        Guid? applicationId,
        ClaimsPrincipal claimsPrincipal,
        Authorization.OpenIdConnectAuthorization authorization, // TODO Make the authorization manager use the scoped database context.
        OpenIddictAuthorizationManager<OpenIdConnectAuthorization> authorizationManager,
        CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToViewApplications(claimsPrincipal, cancellationToken))
        {
            return AsyncEnumerable.Empty<OpenIdConnectAuthorization>();
        }
        if (applicationId is not null)
        {
            return authorizationManager.FindByApplicationIdAsync(applicationId.ToString() ?? "", cancellationToken: cancellationToken);
        }
        return authorizationManager.ListAsync(cancellationToken: cancellationToken);
    }

    [UseUserManager]
    [Authorize(Policy = AuthConfiguration.ReadPolicy)]
    public async Task<OpenIdConnectAuthorization?> GetOpenIdConnectAuthorization(
        Guid authorizationId,
        ClaimsPrincipal claimsPrincipal,
        Authorization.OpenIdConnectAuthorization authorization,
        OpenIddictAuthorizationManager<OpenIdConnectAuthorization> authorizationManager,
        CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToViewApplications(claimsPrincipal, cancellationToken))
        {
            return null;
        }
        return await authorizationManager.FindByIdAsync(authorizationId.ToString(), cancellationToken: cancellationToken);
    }
}