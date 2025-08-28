using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Metabase.Data.OpenIdConnect;
using Metabase.GraphQl.Users;
using OpenIddict.Core;

namespace Metabase.GraphQl.OpenIdConnect.Tokens;

public sealed class OpenIdConnectTokenType
    : ObjectType<OpenIdConnectToken>
{
    protected override void Configure(
        IObjectTypeDescriptor<OpenIdConnectToken> descriptor
    )
    {
        descriptor.Field(token => token.Properties).Ignore();
        descriptor.Field(token => token.ReferenceId).Ignore();
        descriptor.Field(token => token.Payload).Ignore();
        descriptor.Field(token => token.ConcurrencyToken).Ignore();

        descriptor
            .ImplementsNode()
            .IdField(t => t.Id)
            .ResolveNode((context, id) =>
                context
                    .Service<OpenIddictTokenManager<OpenIdConnectToken>>()
                    .FindByIdAsync(id.ToString(), context.RequestAborted)
                    .AsTask()
            );
        descriptor
            .Field("uuid")
            .Type<NonNullType<UuidType>>()
            .Resolve(context =>
                context.Parent<OpenIdConnectToken>().Id
            );

        descriptor
            .Field(t => t.Application)
            .Type<NonNullType<ObjectType<OpenIdConnectTokenApplicationEdge>>>()
            .Resolve(context =>
                new OpenIdConnectTokenApplicationEdge(
                    context.Parent<OpenIdConnectToken>().Application!
                )
            );
        descriptor
            .Field(t => t.Authorization)
            .Type<NonNullType<ObjectType<OpenIdConnectTokenAuthorizationEdge>>>()
            .Resolve(context =>
                new OpenIdConnectTokenAuthorizationEdge(
                    context.Parent<OpenIdConnectToken>().Authorization!
                )
            );
        descriptor
                .Field("canCurrentUserRevokeNode")
                .ResolveWith<TokenResolvers>(x =>
                    TokenResolvers.GetCanCurrentUserRevokeNodeAsync(default!, default!, default!, default!, default!))
                .UseUserManager();
    }

    private sealed class TokenResolvers
    {
        public static Task<bool> GetCanCurrentUserRevokeNodeAsync(
            [Parent] OpenIdConnectToken token,
            ClaimsPrincipal claimsPrincipal,
            Authorization.OpenIdConnectAuthorization authorization,
            OpenIddictTokenManager<OpenIdConnectToken> tokenManager,
            CancellationToken cancellationToken
        )
        {
            return authorization.IsAuthorizedToManageToken(claimsPrincipal, token.Id, tokenManager, cancellationToken);
        }
    }
}