using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Metabase.Data.OpenIdConnect;
using Metabase.GraphQl.Users;
using Metabase.GraphQl.Entities;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;

namespace Metabase.GraphQl.OpenIdConnect.Authorizations;

public sealed class OpenIdConnectAuthorizationType
    : EntityType<OpenIdConnectAuthorization, IOpenIdConnectAuthorizationByIdDataLoader>
{
    protected override void Configure(
        IObjectTypeDescriptor<OpenIdConnectAuthorization> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Field(authorization => authorization.ConcurrencyToken).Ignore();
        descriptor.Field(authorization => authorization.Properties).Ignore();
        descriptor.Field(authorization => authorization.CreationDate).Ignore(); // use `CreatedAt` instead

        descriptor
            .Field(authorization => authorization.Scopes)
            .Type<NonNullType<ListType<NonNullType<EnumType<OpenIdConnectScope>>>>>()
            .Cost(0)
            .Resolve(context =>
        {
            var authorization = context.Parent<OpenIdConnectAuthorization>();
            if (authorization.Scopes is null)
            {
                return [];
            }
            return JsonSerializer.Deserialize<List<string>>(authorization.Scopes)
                ?.Select(scope => scope.ToOpenIdConnectScope())
                .ToList() ?? [];
        });
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
            .Type<NonNullType<ObjectType<OpenIdConnectAuthorizationIssuedTokenConnection>>>()
            .Resolve(context =>
                new OpenIdConnectAuthorizationIssuedTokenConnection(
                    context.Parent<OpenIdConnectAuthorization>()
                )
            );
        descriptor
            .Field("isAuthorizedToDeleteNode")
            .Cost(1)
            .ResolveWith<AuthorizationResolvers>(x =>
                AuthorizationResolvers.IsAuthorizedToDeleteNodeAsync(default!, default!, default!, default!))
            .UseUserManager();
    }

    private sealed class AuthorizationResolvers
    {
        public static Task<bool> IsAuthorizedToDeleteNodeAsync(
            [Parent] OpenIdConnectAuthorization authorization,
            ClaimsPrincipal claimsPrincipal,
            Authorization.OpenIdConnectAuthorization openIdConnectAuthorization,
            CancellationToken cancellationToken
        )
        {
            return openIdConnectAuthorization.IsAuthorizedToManageAuthorization(claimsPrincipal, authorization.Id, cancellationToken);
        }
    }
}