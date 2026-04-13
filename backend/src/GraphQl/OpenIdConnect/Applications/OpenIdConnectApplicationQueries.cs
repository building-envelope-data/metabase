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
    // TODO Make the application manager use the scoped database context.
    [UseUserManager]
    [Authorize(Policy = AuthorizationPolicies.ManageOpenIdConnectScopePolicy)]
    public async IAsyncEnumerable<OpenIdConnectApplication> GetOpenIdConnectApplicationsAsync(
        OpenIddictApplicationManager<OpenIdConnectApplication> applicationManager,
        ClaimsPrincipal claimsPrincipal,
        Authorization.OpenIdConnectAuthorization authorization,
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToManageOpenIdConnect(claimsPrincipal, cancellationToken))
        {
            yield break;
        }
        await foreach (var application in applicationManager.ListAsync(cancellationToken: cancellationToken))
        {
            yield return application;
        }
    }

    [UseUserManager]
    [Authorize(Policy = AuthorizationPolicies.ManageOpenIdConnectScopePolicy)]
    public async Task<OpenIdConnectApplication?> GetOpenIdConnectApplicationAsync(
        Guid id,
        ClaimsPrincipal claimsPrincipal,
        Authorization.OpenIdConnectAuthorization authorization,
        OpenIddictApplicationManager<OpenIdConnectApplication> applicationManager,
        CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToManageApplication(claimsPrincipal, id, cancellationToken))
        {
            return null;
        }
        return await applicationManager.FindByIdAsync(id.ToString(), cancellationToken: cancellationToken);
    }
}