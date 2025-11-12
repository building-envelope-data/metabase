using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Metabase.Data.OpenIdConnect;
using Metabase.GraphQl.Users;
using OpenIddict.Core;

namespace Metabase.GraphQl.OpenIdConnect.Authorizations;

public sealed class OpenIdConnectAuthorizationType
    : ObjectType<OpenIdConnectAuthorization>
{
    protected override void Configure(
        IObjectTypeDescriptor<OpenIdConnectAuthorization> descriptor
    )
    {
        descriptor.Field(authorization => authorization.ConcurrencyToken).Ignore();
        descriptor.Field(authorization => authorization.Properties).Ignore();
        descriptor.Field(authorization => authorization.Scopes).Ignore();

        descriptor
            .ImplementsNode()
            .IdField(t => t.Id)
            .ResolveNode((context, id) =>
                context
                    .Service<OpenIddictAuthorizationManager<OpenIdConnectAuthorization>>()
                    .FindByIdAsync(id.ToString(), context.RequestAborted)
                    .AsTask()
            );
        descriptor
            .Field("uuid")
            .Type<NonNullType<UuidType>>()
            .Resolve(context =>
                context.Parent<OpenIdConnectAuthorization>().Id
            );

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