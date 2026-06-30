using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Metabase.Extensions;
using Metabase.GraphQl.OpenIdConnect.Applications;
using OpenIddict.Abstractions;

namespace Metabase.GraphQl.OpenIdConnect;

public static class OpenIdConnectScopeExtensions
{
    [Pure]
    public static OpenIdConnectScope ToOpenIdConnectScope(this string scope)
    {
        return scope switch
        {
            OpenIddictConstants.Scopes.Address => OpenIdConnectScope.ADDRESS,
            OpenIddictConstants.Scopes.Email => OpenIdConnectScope.EMAIL,
            OpenIddictConstants.Scopes.Phone => OpenIdConnectScope.PHONE,
            OpenIddictConstants.Scopes.Profile => OpenIdConnectScope.PROFILE,
            OpenIddictConstants.Scopes.OfflineAccess => OpenIdConnectScope.OFFLINE_ACCESS,
            OpenIddictConstants.Scopes.OpenId => OpenIdConnectScope.OPEN_ID,
            OpenIddictConstants.Scopes.Roles => OpenIdConnectScope.ROLES,
            Data.OpenIdConnect.OpenIdConnectScope.ReadApiScope => OpenIdConnectScope.READ_API,
            Data.OpenIdConnect.OpenIdConnectScope.WriteApiScope => OpenIdConnectScope.WRITE_API,
            Data.OpenIdConnect.OpenIdConnectScope.AdministrateApiScope => OpenIdConnectScope.ADMINISTRATE_API,
            Data.OpenIdConnect.OpenIdConnectScope.VerifyApiScope => OpenIdConnectScope.VERIFY_API,
            Data.OpenIdConnect.OpenIdConnectScope.SupportApiScope => OpenIdConnectScope.SUPPORT_API,
            Data.OpenIdConnect.OpenIdConnectScope.ManageDatabaseApiScope => OpenIdConnectScope.MANAGE_DATABASE_API,
            Data.OpenIdConnect.OpenIdConnectScope.ManageGnuPgApiScope => OpenIdConnectScope.MANAGE_GNU_PG_API,
            Data.OpenIdConnect.OpenIdConnectScope.ManageInstitutionRepresentativeApiScope => OpenIdConnectScope.MANAGE_INSTITUTION_REPRESENTATIVE_API,
            Data.OpenIdConnect.OpenIdConnectScope.ManageOpenIdConnectApiScope => OpenIdConnectScope.MANAGE_OPEN_ID_CONNECT_API,
            Data.OpenIdConnect.OpenIdConnectScope.ManageUserApiScope => OpenIdConnectScope.MANAGE_USER_API,
            _ => throw new ArgumentOutOfRangeException(nameof(scope), $"Unsupported scope `{scope}`")
        };
    }

    [Pure]
    public static OpenIdConnectScope[] PermissionsToOpenIdConnectScopes(this List<string> permissions)
    {
        var grantTypes = permissions.PermissionsToOpenIdConnectGrantTypes();
        return permissions.FindAll(permission =>
        {
            try
            {
                var ignore = permission.PermissionToOpenIdConnectScope();
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        })
        .Select(scopePermission => scopePermission.PermissionToOpenIdConnectScope())
        .If(
            grantTypes.Contains(OpenIdConnectGrantType.REFRESH_TOKEN),
            _ => _.Prepend(OpenIdConnectScope.OFFLINE_ACCESS)
        )
        .Prepend(OpenIdConnectScope.OPEN_ID)
        .ToArray();
    }

    [Pure]
    public static OpenIdConnectScope PermissionToOpenIdConnectScope(this string scopePermission)
    {
        return scopePermission switch
        {
            OpenIddictConstants.Permissions.Scopes.Address => OpenIdConnectScope.ADDRESS,
            OpenIddictConstants.Permissions.Scopes.Email => OpenIdConnectScope.EMAIL,
            OpenIddictConstants.Permissions.Scopes.Phone => OpenIdConnectScope.PHONE,
            OpenIddictConstants.Permissions.Scopes.Profile => OpenIdConnectScope.PROFILE,
            // The openid and offline_access scopes are special-cased by
            // OpenIddict and don't require explicit permissions according to
            // https://documentation.openiddict.com/configuration/application-permissions#scope-permissions
            // ... => OpenIdConnectScope.OFFLINE_ACCESS,
            // ... => OpenIdConnectScope.OPEN_ID,
            OpenIddictConstants.Permissions.Scopes.Roles => OpenIdConnectScope.ROLES,
            OpenIddictConstants.Permissions.Prefixes.Scope + Data.OpenIdConnect.OpenIdConnectScope.ReadApiScope => OpenIdConnectScope.READ_API,
            OpenIddictConstants.Permissions.Prefixes.Scope + Data.OpenIdConnect.OpenIdConnectScope.WriteApiScope => OpenIdConnectScope.WRITE_API,
            OpenIddictConstants.Permissions.Prefixes.Scope + Data.OpenIdConnect.OpenIdConnectScope.AdministrateApiScope => OpenIdConnectScope.ADMINISTRATE_API,
            OpenIddictConstants.Permissions.Prefixes.Scope + Data.OpenIdConnect.OpenIdConnectScope.VerifyApiScope => OpenIdConnectScope.VERIFY_API,
            OpenIddictConstants.Permissions.Prefixes.Scope + Data.OpenIdConnect.OpenIdConnectScope.SupportApiScope => OpenIdConnectScope.SUPPORT_API,
            OpenIddictConstants.Permissions.Prefixes.Scope + Data.OpenIdConnect.OpenIdConnectScope.ManageDatabaseApiScope => OpenIdConnectScope.MANAGE_DATABASE_API,
            OpenIddictConstants.Permissions.Prefixes.Scope + Data.OpenIdConnect.OpenIdConnectScope.ManageGnuPgApiScope => OpenIdConnectScope.MANAGE_GNU_PG_API,
            OpenIddictConstants.Permissions.Prefixes.Scope + Data.OpenIdConnect.OpenIdConnectScope.ManageInstitutionRepresentativeApiScope => OpenIdConnectScope.MANAGE_INSTITUTION_REPRESENTATIVE_API,
            OpenIddictConstants.Permissions.Prefixes.Scope + Data.OpenIdConnect.OpenIdConnectScope.ManageOpenIdConnectApiScope => OpenIdConnectScope.MANAGE_OPEN_ID_CONNECT_API,
            OpenIddictConstants.Permissions.Prefixes.Scope + Data.OpenIdConnect.OpenIdConnectScope.ManageUserApiScope => OpenIdConnectScope.MANAGE_USER_API,
            _ => throw new ArgumentOutOfRangeException(nameof(scopePermission), $"Unsupported scope `{scopePermission}`")
        };
    }

    [Pure]
    public static IEnumerable<string> ToPermissionStrings(this IEnumerable<OpenIdConnectScope> scopes)
    {
        return scopes
            // The openid and offline_access scopes are special-cased by
            // OpenIddict and don't require explicit permissions according to
            // https://documentation.openiddict.com/configuration/application-permissions#scope-permissions
            .Where(_ => _ is not OpenIdConnectScope.OFFLINE_ACCESS && _ is not OpenIdConnectScope.OPEN_ID)
            .Select(_ => _.ToPermissionString());
    }

    [Pure]
    private static string ToPermissionString(this OpenIdConnectScope scope)
    {
        return scope switch
        {
            OpenIdConnectScope.ADDRESS => OpenIddictConstants.Permissions.Scopes.Address,
            OpenIdConnectScope.EMAIL => OpenIddictConstants.Permissions.Scopes.Email,
            OpenIdConnectScope.PHONE => OpenIddictConstants.Permissions.Scopes.Phone,
            OpenIdConnectScope.PROFILE => OpenIddictConstants.Permissions.Scopes.Profile,
            // The openid and offline_access scopes are special-cased by
            // OpenIddict and don't require explicit permissions according to
            // https://documentation.openiddict.com/configuration/application-permissions#scope-permissions
            // OpenIdConnectScope.OFFLINE_ACCESS => ...,
            // OpenIdConnectScope.OPEN_ID => ...,
            OpenIdConnectScope.ROLES => OpenIddictConstants.Permissions.Scopes.Roles,
            OpenIdConnectScope.READ_API => OpenIddictConstants.Permissions.Prefixes.Scope + Data.OpenIdConnect.OpenIdConnectScope.ReadApiScope,
            OpenIdConnectScope.WRITE_API => OpenIddictConstants.Permissions.Prefixes.Scope + Data.OpenIdConnect.OpenIdConnectScope.WriteApiScope,
            OpenIdConnectScope.ADMINISTRATE_API => OpenIddictConstants.Permissions.Prefixes.Scope + Data.OpenIdConnect.OpenIdConnectScope.AdministrateApiScope,
            OpenIdConnectScope.VERIFY_API => OpenIddictConstants.Permissions.Prefixes.Scope + Data.OpenIdConnect.OpenIdConnectScope.VerifyApiScope,
            OpenIdConnectScope.SUPPORT_API => OpenIddictConstants.Permissions.Prefixes.Scope + Data.OpenIdConnect.OpenIdConnectScope.SupportApiScope,
            OpenIdConnectScope.MANAGE_DATABASE_API => OpenIddictConstants.Permissions.Prefixes.Scope + Data.OpenIdConnect.OpenIdConnectScope.ManageDatabaseApiScope,
            OpenIdConnectScope.MANAGE_GNU_PG_API => OpenIddictConstants.Permissions.Prefixes.Scope + Data.OpenIdConnect.OpenIdConnectScope.ManageGnuPgApiScope,
            OpenIdConnectScope.MANAGE_INSTITUTION_REPRESENTATIVE_API => OpenIddictConstants.Permissions.Prefixes.Scope + Data.OpenIdConnect.OpenIdConnectScope.ManageInstitutionRepresentativeApiScope,
            OpenIdConnectScope.MANAGE_OPEN_ID_CONNECT_API => OpenIddictConstants.Permissions.Prefixes.Scope + Data.OpenIdConnect.OpenIdConnectScope.ManageOpenIdConnectApiScope,
            OpenIdConnectScope.MANAGE_USER_API => OpenIddictConstants.Permissions.Prefixes.Scope + Data.OpenIdConnect.OpenIdConnectScope.ManageUserApiScope,
            _ => throw new ArgumentOutOfRangeException(nameof(scope), $"Unsupported scope `{scope}`")
        };
    }
}