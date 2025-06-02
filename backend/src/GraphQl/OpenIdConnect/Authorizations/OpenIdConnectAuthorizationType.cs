using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Metabase.Data;
using Metabase.Data.OpenIdConnect;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Core;

namespace Metabase.GraphQl.OpenIdConnect.Authorizations;

public sealed class OpenIdConnectAuthorizationType
    : ObjectType<OpenIdConnectAuthorization>
{
    protected override void Configure(
        IObjectTypeDescriptor<OpenIdConnectAuthorization> descriptor
    )
    {
        descriptor.Field(authorization => authorization.Application).Ignore();
        descriptor.Field(authorization => authorization.ConcurrencyToken).Ignore();
        descriptor.Field(authorization => authorization.Properties).Ignore();
        descriptor.Field(authorization => authorization.Scopes).Ignore();
        descriptor.Field(authorization => authorization.Tokens).Ignore();

        descriptor
                .Field("canCurrentUserDeleteAuthorization")
                .ResolveWith<AuthorizationResolvers>(x =>
                    AuthorizationResolvers.GetCanCurrentUserDeleteAuthorizationAsync(default!, default!, default!, default!, default!,
                        default!))
                .UseUserManager();
    }

    private sealed class AuthorizationResolvers
    {
        public static Task<bool> GetCanCurrentUserDeleteAuthorizationAsync(
            [Parent] OpenIdConnectAuthorization authorization,
            OpenIddictAuthorizationManager<OpenIdConnectAuthorization> authorizationManager,
            ClaimsPrincipal claimsPrincipal,
            UserManager<User> userManager,
            ApplicationDbContext context,
            CancellationToken cancellationToken
        )
        {
            return Authorization.OpenIdConnectAuthorization.IsAuthorizedToDeleteAuthorization(authorization.Id, authorizationManager, claimsPrincipal, userManager, context, cancellationToken);
        }
    }
}