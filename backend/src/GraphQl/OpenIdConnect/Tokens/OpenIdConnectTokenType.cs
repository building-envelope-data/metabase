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

namespace Metabase.GraphQl.OpenIdConnect.Tokens;

public sealed class OpenIdConnectTokenType
    : ObjectType<OpenIdConnectToken>
{
    protected override void Configure(
        IObjectTypeDescriptor<OpenIdConnectToken> descriptor
    )
    {
        descriptor.Field(token => token.Application).Ignore();
        descriptor.Field(token => token.Authorization).Ignore();
        descriptor.Field(token => token.Properties).Ignore();
        descriptor.Field(token => token.ReferenceId).Ignore();
        descriptor.Field(token => token.Payload).Ignore();
        descriptor.Field(token => token.ConcurrencyToken).Ignore();

        descriptor
                .Field("canCurrentUserRevokeToken")
                .ResolveWith<TokenResolvers>(x =>
                    TokenResolvers.GetCanCurrentUserRevokeTokenAsync(default!, default!, default!, default!, default!))
                .UseUserManager();
    }

    private sealed class TokenResolvers
    {
        public static Task<bool> GetCanCurrentUserRevokeTokenAsync(
            [Parent] OpenIdConnectToken token,
            ClaimsPrincipal claimsPrincipal,
            Authorization.OpenIdConnectAuthorization authorization,
            OpenIddictTokenManager<OpenIdConnectToken> tokenManager,
            CancellationToken cancellationToken
        )
        {
            return authorization.IsAuthorizedToRevokeToken(claimsPrincipal, token.Id, tokenManager, cancellationToken);
        }
    }
}