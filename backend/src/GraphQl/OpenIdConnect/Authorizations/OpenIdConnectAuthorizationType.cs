using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Metabase.Data.OpenIdConnect;
using Metabase.GraphQl.Users;
using Metabase.GraphQl.Entities;
using OpenIddict.Core;

namespace Metabase.GraphQl.OpenIdConnect.Authorizations;

public sealed class OpenIdConnectAuthorizationType
    : EntityType<OpenIdConnectAuthorization, OpenIdConnectAuthorizationByIdDataLoader>
{
    protected override void Configure(
        IObjectTypeDescriptor<OpenIdConnectAuthorization> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Field(authorization => authorization.ConcurrencyToken).Ignore();
        descriptor.Field(authorization => authorization.Properties).Ignore();
        descriptor.Field(authorization => authorization.Scopes).Ignore();

        descriptor
            .Field(t => t.Application)
            .Type<NonNullType<ObjectType<OpenIdConnectAuthorizationApplicationEdge>>>()
            .Resolve(context =>
                new OpenIdConnectAuthorizationApplicationEdge(
                    context.Parent<OpenIdConnectAuthorization>().Application!
                )
            );
        descriptor
            .Field(authorization => authorization.Tokens)
            .Type<NonNullType<ObjectType<OpenIdConnectAuthorizationTokenConnection>>>()
            .Resolve(context =>
                new OpenIdConnectAuthorizationTokenConnection(
                    context.Parent<OpenIdConnectAuthorization>()
                )
            );
        descriptor
                .Field("isAuthorizedToDeleteNode")
                .ResolveWith<AuthorizationResolvers>(x =>
                    AuthorizationResolvers.IsAuthorizedToDeleteNodeAsync(default!, default!, default!, default!, default!))
                .UseUserManager();
    }

    private sealed class AuthorizationResolvers
    {
        public static Task<bool> IsAuthorizedToDeleteNodeAsync(
            [Parent] OpenIdConnectAuthorization authorization,
            ClaimsPrincipal claimsPrincipal,
            Authorization.OpenIdConnectAuthorization openIdConnectAuthorization,
            OpenIddictAuthorizationManager<OpenIdConnectAuthorization> authorizationManager,
            CancellationToken cancellationToken
        )
        {
            return openIdConnectAuthorization.IsAuthorizedToManageAuthorization(claimsPrincipal, authorization.Id, authorizationManager, cancellationToken);
        }
    }
}