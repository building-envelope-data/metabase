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
                    AuthorizationResolvers.GetCanCurrentUserDeleteAuthorizationAsync(default!, default!, default!, default!, default!))
                .UseUserManager();
    }

    private sealed class AuthorizationResolvers
    {
        public static Task<bool> GetCanCurrentUserDeleteAuthorizationAsync(
            [Parent] OpenIdConnectAuthorization authorization,
            ClaimsPrincipal claimsPrincipal,
            Authorization.OpenIdConnectAuthorization openIdConnectAuthorization,
            OpenIddictAuthorizationManager<OpenIdConnectAuthorization> authorizationManager,
            CancellationToken cancellationToken
        )
        {
            return openIdConnectAuthorization.IsAuthorizedToDeleteAuthorization(claimsPrincipal, authorization.Id, authorizationManager, cancellationToken);
        }
    }
}