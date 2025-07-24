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
        descriptor.Field(authorization => authorization.Application).Ignore();
        descriptor.Field(authorization => authorization.ConcurrencyToken).Ignore();
        descriptor.Field(authorization => authorization.Properties).Ignore();
        descriptor.Field(authorization => authorization.Scopes).Ignore();
        descriptor.Field(authorization => authorization.Tokens).Ignore();

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
                .Field("canCurrentUserDeleteNode")
                .ResolveWith<AuthorizationResolvers>(x =>
                    AuthorizationResolvers.GetCanCurrentUserDeleteNodeAsync(default!, default!, default!, default!, default!))
                .UseUserManager();
    }

    private sealed class AuthorizationResolvers
    {
        public static Task<bool> GetCanCurrentUserDeleteNodeAsync(
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