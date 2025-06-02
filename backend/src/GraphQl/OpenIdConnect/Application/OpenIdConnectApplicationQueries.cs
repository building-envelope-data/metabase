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

namespace Metabase.GraphQl.OpenIdConnect.Application;

[ExtendObjectType(nameof(Query))]
public sealed class OpenIdConnectApplicationQueries
{
    // TODO In all queries, instead of returning nothing, report as authentication error to client.
    // TODO Make the application manager use the scoped database context.
    [UseUserManager]
    [Authorize(Policy = AuthConfiguration.ReadPolicy)]
    public async Task<IAsyncEnumerable<OpenIdConnectApplication>> GetOpenIdConnectApplicationsAsync(
        OpenIddictApplicationManager<OpenIdConnectApplication> applicationManager,
        ClaimsPrincipal claimsPrincipal,
        UserManager<User> userManager,
        ApplicationDbContext context,
        CancellationToken cancellationToken
    )
    {
        if (!await Authorization.OpenIdConnectAuthorization.IsAuthorizedToViewApplications(claimsPrincipal, userManager, context, cancellationToken)
                .ConfigureAwait(false))
        {
            return AsyncEnumerable.Empty<OpenIdConnectApplication>();
        }
        return applicationManager.ListAsync(cancellationToken: cancellationToken);
    }

    [UseUserManager]
    [Authorize(Policy = AuthConfiguration.ReadPolicy)]
    public async Task<OpenIdConnectApplication?> GetOpenIdConnectApplicationAsync(
        Guid applicationId,
        OpenIddictApplicationManager<OpenIdConnectApplication> applicationManager,
        ClaimsPrincipal claimsPrincipal,
        UserManager<User> userManager,
        ApplicationDbContext context,
        CancellationToken cancellationToken
    )
    {
        if (!await Authorization.OpenIdConnectAuthorization.IsAuthorizedToViewApplications(claimsPrincipal, userManager, context, cancellationToken)
                .ConfigureAwait(false))
        {
            return null;
        }
        return await applicationManager.FindByIdAsync(applicationId.ToString(), cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}