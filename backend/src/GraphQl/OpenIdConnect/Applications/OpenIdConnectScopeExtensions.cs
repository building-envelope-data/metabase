using System;
using OpenIddict.Abstractions;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

public static class OpenIdConnectScopeExtensions
{
    public static OpenIdConnectScope ToOpenIdConnectScope(this string scope)
    {
        return scope switch
        {
            OpenIddictConstants.Permissions.Scopes.Address => OpenIdConnectScope.ADDRESS,
            OpenIddictConstants.Permissions.Scopes.Email => OpenIdConnectScope.EMAIL,
            OpenIddictConstants.Permissions.Scopes.OfflineAccess => OpenIdConnectScope.OFFLINE_ACCESS,
            OpenIddictConstants.Permissions.Scopes.OpenId => OpenIdConnectScope.OPEN_ID,
            OpenIddictConstants.Permissions.Scopes.Phone => OpenIdConnectScope.PHONE,
            OpenIddictConstants.Permissions.Scopes.Profile => OpenIdConnectScope.PROFILE,
            OpenIddictConstants.Permissions.Scopes.Roles => OpenIdConnectScope.ROLES,
            OpenIddictConstants.Permissions.Prefixes.Scope + Data.OpenIdConnect.OpenIdConnectScope.ReadApiScope => OpenIdConnectScope.READ_API,
            OpenIddictConstants.Permissions.Prefixes.Scope + Data.OpenIdConnect.OpenIdConnectScope.WriteApiScope => OpenIdConnectScope.WRITE_API,
            OpenIddictConstants.Permissions.Prefixes.Scope + Data.OpenIdConnect.OpenIdConnectScope.AdministrateApiScope => OpenIdConnectScope.ADMINISTRATE_API,
            OpenIddictConstants.Permissions.Prefixes.Scope + Data.OpenIdConnect.OpenIdConnectScope.VerifyApiScope => OpenIdConnectScope.VERIFY_API,
            OpenIddictConstants.Permissions.Prefixes.Scope + Data.OpenIdConnect.OpenIdConnectScope.ManageDatabaseApiScope => OpenIdConnectScope.MANAGE_DATABASE_API,
            OpenIddictConstants.Permissions.Prefixes.Scope + Data.OpenIdConnect.OpenIdConnectScope.ManageGnuPgApiScope => OpenIdConnectScope.MANAGE_GNU_PG_API,
            OpenIddictConstants.Permissions.Prefixes.Scope + Data.OpenIdConnect.OpenIdConnectScope.ManageInstitutionRepresentativeApiScope => OpenIdConnectScope.MANAGE_INSTITUTION_REPRESENTATIVE_API,
            OpenIddictConstants.Permissions.Prefixes.Scope + Data.OpenIdConnect.OpenIdConnectScope.ManageOpenIdConnectApiScope => OpenIdConnectScope.MANAGE_OPEN_ID_CONNECT_API,
            OpenIddictConstants.Permissions.Prefixes.Scope + Data.OpenIdConnect.OpenIdConnectScope.ManageUserApiScope => OpenIdConnectScope.MANAGE_USER_API,
            _ => throw new ArgumentOutOfRangeException(nameof(scope), $"Unsupported scope `{scope}`")
        };
    }

    public static string ToStringScope(this OpenIdConnectScope scope)
    {
        return scope switch
        {
            OpenIdConnectScope.ADDRESS => OpenIddictConstants.Permissions.Scopes.Address,
            OpenIdConnectScope.EMAIL => OpenIddictConstants.Permissions.Scopes.Email,
            OpenIdConnectScope.OFFLINE_ACCESS => OpenIddictConstants.Permissions.Scopes.OfflineAccess,
            OpenIdConnectScope.OPEN_ID => OpenIddictConstants.Permissions.Scopes.OpenId,
            OpenIdConnectScope.PHONE => OpenIddictConstants.Permissions.Scopes.Phone,
            OpenIdConnectScope.PROFILE => OpenIddictConstants.Permissions.Scopes.Profile,
            OpenIdConnectScope.ROLES => OpenIddictConstants.Permissions.Scopes.Roles,
            OpenIdConnectScope.READ_API => OpenIddictConstants.Permissions.Prefixes.Scope + Data.OpenIdConnect.OpenIdConnectScope.ReadApiScope,
            OpenIdConnectScope.WRITE_API => OpenIddictConstants.Permissions.Prefixes.Scope + Data.OpenIdConnect.OpenIdConnectScope.WriteApiScope,
            OpenIdConnectScope.ADMINISTRATE_API => OpenIddictConstants.Permissions.Prefixes.Scope + Data.OpenIdConnect.OpenIdConnectScope.AdministrateApiScope,
            OpenIdConnectScope.VERIFY_API => OpenIddictConstants.Permissions.Prefixes.Scope + Data.OpenIdConnect.OpenIdConnectScope.VerifyApiScope,
            OpenIdConnectScope.MANAGE_DATABASE_API => OpenIddictConstants.Permissions.Prefixes.Scope + Data.OpenIdConnect.OpenIdConnectScope.ManageDatabaseApiScope,
            OpenIdConnectScope.MANAGE_GNU_PG_API => OpenIddictConstants.Permissions.Prefixes.Scope + Data.OpenIdConnect.OpenIdConnectScope.ManageGnuPgApiScope,
            OpenIdConnectScope.MANAGE_INSTITUTION_REPRESENTATIVE_API => OpenIddictConstants.Permissions.Prefixes.Scope + Data.OpenIdConnect.OpenIdConnectScope.ManageInstitutionRepresentativeApiScope,
            OpenIdConnectScope.MANAGE_OPEN_ID_CONNECT_API => OpenIddictConstants.Permissions.Prefixes.Scope + Data.OpenIdConnect.OpenIdConnectScope.ManageOpenIdConnectApiScope,
            OpenIdConnectScope.MANAGE_USER_API => OpenIddictConstants.Permissions.Prefixes.Scope + Data.OpenIdConnect.OpenIdConnectScope.ManageUserApiScope,
            _ => throw new ArgumentOutOfRangeException(nameof(scope), $"Unsupported scope `{scope}`")
        };
    }
}