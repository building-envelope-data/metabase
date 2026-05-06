using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Metabase.Data.OpenIdConnect;
using Metabase.GraphQl.Users;
using Metabase.GraphQl.Entities;

namespace Metabase.GraphQl.OpenIdConnect.Tokens;

public sealed class OpenIdConnectTokenType
    : EntityType<OpenIdConnectToken, IOpenIdConnectTokenByIdDataLoader>
{
    internal const string ExpiredAtName = "expiredAt";
    internal const string RedeemedAtName = "redeemedAt";

    protected override void Configure(
        IObjectTypeDescriptor<OpenIdConnectToken> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Field(token => token.Properties).Ignore();
        descriptor.Field(token => token.ReferenceId).Ignore();
        descriptor.Field(token => token.Payload).Ignore();
        descriptor.Field(token => token.ConcurrencyToken).Ignore();
        descriptor.Field(token => token.CreationDate).Ignore(); // use `CreatedAt` instead

        descriptor
            .Field(_ => _.ExpirationDate)
            .Name(ExpiredAtName);
        descriptor
            .Field(_ => _.RedemptionDate)
            .Name(RedeemedAtName);
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
            .Cost(1)
            .ResolveWith<TokenResolvers>(x =>
                TokenResolvers.IsAuthorizedToRevokeNodeAsync(default!, default!, default!, default!))
            .UseUserManager();
    }

    private sealed class TokenResolvers
    {
        public static Task<bool> IsAuthorizedToRevokeNodeAsync(
            [Parent] OpenIdConnectToken token,
            ClaimsPrincipal claimsPrincipal,
            Authorization.OpenIdConnectAuthorization authorization,
            CancellationToken cancellationToken
        )
        {
            return authorization.IsAuthorizedToManageToken(claimsPrincipal, token.Id, cancellationToken);
        }
    }
}