using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Metabase.Data;
using Metabase.Data.OpenIdConnect;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Core;

namespace Metabase.GraphQl.OpenIdConnect.Application;

public sealed class OpenIdConnectApplicationType
    : ObjectType<OpenIdConnectApplication>
{
    protected override void Configure(
        IObjectTypeDescriptor<OpenIdConnectApplication> descriptor
    )
    {
        descriptor.Field(application => application.ApplicationType).Ignore();
        descriptor.Field(application => application.Authorizations).Ignore();
        descriptor.Field(application => application.ClientType).Ignore();
        descriptor.Field(application => application.ClientSecret).Ignore();
        descriptor.Field(application => application.ConcurrencyToken).Ignore();
        descriptor.Field(application => application.DisplayNames).Ignore();
        descriptor.Field(application => application.InstitutionEdges).Ignore();
        descriptor.Field(application => application.JsonWebKeySet).Ignore();
        descriptor.Field(application => application.Properties).Ignore();
        descriptor.Field(application => application.Requirements).Ignore();
        descriptor.Field(application => application.Settings).Ignore();
        descriptor.Field(application => application.Tokens).Ignore();

        descriptor
            .Field(application => application.ConsentType)
            .Type<NonNullType<EnumType<OpenIdConnectConsentType>>>()
            .Resolve(context =>
                context.Parent<OpenIdConnectApplication>().ConsentType?.ToOpenIdConnectConsentType()
                ?? throw new GraphQLException("Consent type is missing.")
            );
        descriptor
            .Field(application => application.Permissions)
            .Name("scopes")
            .Type<NonNullType<ListType<EnumType<OpenIdConnectScope>>>>()
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
                        permission.ToOpenIdConnectScope();
                        return true;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        return false;
                    }
                })
                ?.Select(scope => scope.ToOpenIdConnectScope())
                .ToList();
        });
        descriptor
            .Field(application => application.RedirectUris)
            .Name("redirectUri")
            .Type<UrlType>()
            .Resolve(context => ExtractUri(context.Parent<OpenIdConnectApplication>().RedirectUris));
        descriptor
            .Field(application => application.PostLogoutRedirectUris)
            .Name("postLogoutRedirectUri")
            .Type<UrlType>()
            .Resolve(context => ExtractUri(context.Parent<OpenIdConnectApplication>().PostLogoutRedirectUris));
        descriptor
            .Field(application => application.Institutions)
            .Type<NonNullType<ObjectType<OpenIdConnectApplicationInstitutionConnection>>>()
            .Resolve(context =>
                new OpenIdConnectApplicationInstitutionConnection(
                    context.Parent<OpenIdConnectApplication>()
                )
            );

        descriptor
            .Field("canCurrentUserManageApplication")
            .ResolveWith<ApplicationResolvers>(_ =>
                ApplicationResolvers.GetCanCurrentUserManageApplicationAsync(default!, default!, default!, default!))
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
        public static Task<bool> GetCanCurrentUserManageApplicationAsync(
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