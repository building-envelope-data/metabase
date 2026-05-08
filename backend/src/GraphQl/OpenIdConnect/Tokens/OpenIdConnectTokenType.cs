using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Metabase.Data.OpenIdConnect;
using Metabase.GraphQl.Users;
using Metabase.GraphQl.Entities;
using Metabase.Data;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Core;

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
            .Field(_ => _.Subject)
            .Type<UnionType<IOpenIdConnectSubject>>()
            .Cost(1)
            .ResolveWith<TokenResolvers>(x =>
                TokenResolvers.GetSubjectAsync(default!, default!, default!, default!)
            );
        descriptor
            .Field(t => t.Application)
            .Type<ObjectType<OpenIdConnectTokenApplicationEdge>>()
            .Resolve(context =>
            {
                // auto-included in `ApplicationDbContext`
                var application = context.Parent<OpenIdConnectToken>().Application;
                return application is null
                    ? null
                    : new OpenIdConnectTokenApplicationEdge(application);
            });
        descriptor
            .Field(t => t.Authorization)
            .Type<ObjectType<OpenIdConnectTokenAuthorizationEdge>>()
            .Resolve(context =>
            {
                // auto-included in `ApplicationDbContext`
                var authorization = context.Parent<OpenIdConnectToken>().Authorization;
                return authorization is null
                    ? null
                    : new OpenIdConnectTokenAuthorizationEdge(authorization);
            });
        descriptor
            .Field("isAuthorizedToRevokeNode")
            .Cost(1)
            .ResolveWith<TokenResolvers>(x =>
                TokenResolvers.IsAuthorizedToRevokeNodeAsync(default!, default!, default!, default!)
            )
            .UseUserManager();
    }

    private sealed class TokenResolvers
    {
        public static Task<IOpenIdConnectSubject?> GetSubjectAsync(
            [Parent] OpenIdConnectToken token,
            OpenIddictApplicationManager<OpenIdConnectApplication> applicationManager,
            ApplicationDbContext databaseContext,
            CancellationToken cancellationToken
        )
        {
            return IOpenIdConnectSubject.SwitchSubjectAsync<IOpenIdConnectSubject?>(
                token.Subject,
                async (userId) =>
                    await databaseContext.Users.AsNoTracking()
                    .SingleOrDefaultAsync(_ => _.Id == userId, cancellationToken),
                async (clientId) =>
                    await applicationManager.FindByClientIdAsync(clientId, cancellationToken),
                async () => null
            );
        }

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