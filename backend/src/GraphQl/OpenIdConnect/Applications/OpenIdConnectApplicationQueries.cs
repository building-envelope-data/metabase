using System;
using System.Collections.Generic;
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

namespace Metabase.GraphQl.OpenIdConnect.Applications;

[ExtendObjectType(nameof(Query))]
public sealed class OpenIdConnectApplicationQueries
{
    // TODO In all queries, instead of returning nothing, report as authentication error to client.
    // TODO Make the application manager use the scoped database context.
    [UseUserManager]
    [Authorize(Policy = AuthConfiguration.ReadPolicy)]
    public async IAsyncEnumerable<OpenIdConnectApplication> GetOpenIdConnectApplicationsAsync(
        OpenIddictApplicationManager<OpenIdConnectApplication> applicationManager, // TODO Make the application manager use the scoped database context.
        ClaimsPrincipal claimsPrincipal,
        Authorization.OpenIdConnectAuthorization authorization,
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToManage(claimsPrincipal, cancellationToken))
        {
            yield break;
        }
        await foreach (var application in applicationManager.ListAsync(cancellationToken: cancellationToken))
        {
            yield return application;
        }
    }

    [UseUserManager]
    [Authorize(Policy = AuthConfiguration.ReadPolicy)]
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