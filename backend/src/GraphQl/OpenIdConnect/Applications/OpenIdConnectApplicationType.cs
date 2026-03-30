using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Metabase.Data.OpenIdConnect;
using Metabase.GraphQl.Users;
using Metabase.GraphQl.Entities;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

public sealed class OpenIdConnectApplicationType
    : EntityType<OpenIdConnectApplication, IOpenIdConnectApplicationByIdDataLoader>
{
    protected override void Configure(
        IObjectTypeDescriptor<OpenIdConnectApplication> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Field(application => application.ClientSecret).Ignore();
        descriptor.Field(application => application.ConcurrencyToken).Ignore();
        descriptor.Field(application => application.DisplayNames).Ignore();
        descriptor.Field(application => application.JsonWebKeySet).Ignore();
        descriptor.Field(application => application.Properties).Ignore();
        descriptor.Field(application => application.Settings).Ignore();

        descriptor
            .Field(application => application.ClientId)
            .Type<NonNullType<StringType>>()
            .Cost(0)
            .Resolve(context =>
                context.Parent<OpenIdConnectApplication>().ClientId
                ?? throw new GraphQLException("Client ID is missing.")
            );
        descriptor
            .Field(application => application.ConsentType)
            .Type<NonNullType<EnumType<OpenIdConnectConsentType>>>()
            .Cost(0)
            .Resolve(context =>
                context.Parent<OpenIdConnectApplication>().ConsentType?.ToOpenIdConnectConsentType()
                ?? throw new GraphQLException("Consent type is missing.")
            );
        descriptor
            .Field(application => application.Permissions)
            .Ignore();
        descriptor
            .Field("endpoints")
            .Type<NonNullType<ListType<NonNullType<EnumType<OpenIdConnectEndpoint>>>>>()
            .Cost(0)
            .Resolve(context =>
        {
            var application = context.Parent<OpenIdConnectApplication>();
            if (application.Permissions is null)
            {
                return [];
            }
            return JsonSerializer.Deserialize<List<string>>(application.Permissions)
                ?.FindAll(permission =>
                {
                    try
                    {
                        permission.PermissionToOpenIdConnectEndpoint();
                        return true;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        return false;
                    }
                })
                ?.Select(endpointPermission => endpointPermission.PermissionToOpenIdConnectEndpoint())
                .ToList() ?? [];
        });
        descriptor
            .Field("grantTypes")
            .Type<NonNullType<ListType<NonNullType<EnumType<OpenIdConnectGrantType>>>>>()
            .Cost(0)
            .Resolve(context =>
        {
            var application = context.Parent<OpenIdConnectApplication>();
            if (application.Permissions is null)
            {
                return [];
            }
            return JsonSerializer.Deserialize<List<string>>(application.Permissions)
                ?.FindAll(permission =>
                {
                    try
                    {
                        permission.PermissionToOpenIdConnectGrantType();
                        return true;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        return false;
                    }
                })
                ?.Select(grantTypePermission => grantTypePermission.PermissionToOpenIdConnectGrantType())
                .ToList() ?? [];
        });
        descriptor
            .Field("responseTypes")
            .Type<NonNullType<ListType<NonNullType<EnumType<OpenIdConnectResponseType>>>>>()
            .Cost(0)
            .Resolve(context =>
        {
            var application = context.Parent<OpenIdConnectApplication>();
            if (application.Permissions is null)
            {
                return [];
            }
            return JsonSerializer.Deserialize<List<string>>(application.Permissions)
                ?.FindAll(permission =>
                {
                    try
                    {
                        permission.PermissionToOpenIdConnectResponseType();
                        return true;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        return false;
                    }
                })
                ?.Select(responseTypePermission => responseTypePermission.PermissionToOpenIdConnectResponseType())
                .ToList() ?? [];
        });
        descriptor
            .Field("scopes")
            .Type<NonNullType<ListType<NonNullType<EnumType<OpenIdConnectScope>>>>>()
            .Cost(0)
            .Resolve(context =>
        {
            var application = context.Parent<OpenIdConnectApplication>();
            if (application.Permissions is null)
            {
                return [];
            }
            return JsonSerializer.Deserialize<List<string>>(application.Permissions)
                ?.FindAll(permission =>
                {
                    try
                    {
                        permission.PermissionToOpenIdConnectScope();
                        return true;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        return false;
                    }
                })
                ?.Select(scopePermission => scopePermission.PermissionToOpenIdConnectScope())
                .ToList() ?? [];
        });
        descriptor
            .Field(application => application.Requirements)
            .Type<NonNullType<ListType<NonNullType<EnumType<OpenIdConnectRequirement>>>>>()
            .Cost(0)
            .Resolve(context =>
        {
            var application = context.Parent<OpenIdConnectApplication>();
            if (application.Requirements is null)
            {
                return [];
            }
            return JsonSerializer.Deserialize<List<string>>(application.Requirements)
                ?.Select(requirement => requirement.ToOpenIdConnectRequirement())
                .ToList() ?? [];
        });
        descriptor
            .Field(application => application.RedirectUris)
            .Name("redirectUri")
            .Type<MyUriType>()
            .Cost(0)
            .Resolve(context => ExtractUri(context.Parent<OpenIdConnectApplication>().RedirectUris));
        descriptor
            .Field(application => application.PostLogoutRedirectUris)
            .Name("postLogoutRedirectUri")
            .Type<MyUriType>()
            .Cost(0)
            .Resolve(context => ExtractUri(context.Parent<OpenIdConnectApplication>().PostLogoutRedirectUris));
        descriptor
            .Field(application => application.Owner)
            .Type<NonNullType<ObjectType<OpenIdConnectApplicationOwnerEdge>>>()
            .Cost(0)
            .Resolve(context =>
                new OpenIdConnectApplicationOwnerEdge(
                    context.Parent<OpenIdConnectApplication>()
                )
            );
        descriptor
            .Field(application => application.OwnerId)
            .Ignore();
        descriptor
            .Field(application => application.Authorizations)
            .Type<NonNullType<ObjectType<OpenIdConnectApplicationGrantedAuthorizationConnection>>>()
            .Cost(0)
            .Resolve(context =>
                new OpenIdConnectApplicationGrantedAuthorizationConnection(
                    context.Parent<OpenIdConnectApplication>()
                )
            );
        descriptor
            .Field(application => application.Tokens)
            .Type<NonNullType<ObjectType<OpenIdConnectApplicationIssuedTokenConnection>>>()
            .Resolve(context =>
                new OpenIdConnectApplicationIssuedTokenConnection(
                    context.Parent<OpenIdConnectApplication>()
                )
            );

        descriptor
            .Field("isAuthorizedToManageNode")
            .Cost(1)
            .ResolveWith<ApplicationResolvers>(_ =>
                ApplicationResolvers.IsAuthorizedToManageNodeAsync(default!, default!, default!, default!))
            .UseUserManager();
    }

    private static Uri? ExtractUri(string? urisJson)
    {
        if (urisJson is null)
        {
            return null;
        }
        var uris = JsonSerializer.Deserialize<List<string>>(urisJson)
            ?? throw new GraphQLException($"Could not deserialize `{urisJson}` into a list of strings.");
        if (uris.Count == 0)
        {
            return null;
        }
        if (uris.Count >= 2)
        {
            throw new GraphQLException($"There is more than one URI, namely {string.Join(" ,", uris)}.");
        }
        return new Uri(uris[0]);
    }

    private sealed class ApplicationResolvers
    {
        public static Task<bool> IsAuthorizedToManageNodeAsync(
            [Parent] OpenIdConnectApplication application,
            ClaimsPrincipal claimsPrincipal,
            Authorization.OpenIdConnectAuthorization openIdConnectAuthorization,
            CancellationToken cancellationToken
        )
        {
            return openIdConnectAuthorization.IsAuthorizedToManageApplication(claimsPrincipal, application.Id, cancellationToken);
        }
    }
}