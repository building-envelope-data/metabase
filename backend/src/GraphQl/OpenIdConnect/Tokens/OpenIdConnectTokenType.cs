using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Metabase.Data.OpenIdConnect;
using Metabase.GraphQl.Users;
using Metabase.GraphQl.Entities;
using OpenIddict.Core;

namespace Metabase.GraphQl.OpenIdConnect.Tokens;

public sealed class OpenIdConnectTokenType
    : EntityType<OpenIdConnectToken, OpenIdConnectTokenByIdDataLoader>
{
    protected override void Configure(
        IObjectTypeDescriptor<OpenIdConnectToken> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Field(token => token.Properties).Ignore();
        descriptor.Field(token => token.ReferenceId).Ignore();
        descriptor.Field(token => token.Payload).Ignore();
        descriptor.Field(token => token.ConcurrencyToken).Ignore();

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
                .Field("isAuthorizedToRevokeNode")
                .ResolveWith<TokenResolvers>(x =>
                    TokenResolvers.IsAuthorizedToRevokeNodeAsync(default!, default!, default!, default!, default!))
                .UseUserManager();
    }

    private sealed class TokenResolvers
    {
        public static Task<bool> IsAuthorizedToRevokeNodeAsync(
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