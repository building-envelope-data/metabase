using System;
using Metabase.Configuration;
using OpenIddict.Abstractions;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

public static class OpenIdConnectScopeExtensions
{
    public static OpenIdConnectScope ToOpenIdConnectScope(this string scope)
    {
        return scope switch
        {
            OpenIddictConstants.Permissions.Prefixes.Scope + AuthConfiguration.ReadApiScope => OpenIdConnectScope.READ_API,
            OpenIddictConstants.Permissions.Prefixes.Scope + AuthConfiguration.WriteApiScope => OpenIdConnectScope.WRITE_API,
            OpenIddictConstants.Permissions.Prefixes.Scope + AuthConfiguration.ManageUserApiScope => OpenIdConnectScope.MANAGE_USER_API,
            OpenIddictConstants.Permissions.Scopes.Address => OpenIdConnectScope.ADDRESS,
            OpenIddictConstants.Permissions.Scopes.Email => OpenIdConnectScope.EMAIL,
            OpenIddictConstants.Permissions.Scopes.Phone => OpenIdConnectScope.PHONE,
            OpenIddictConstants.Permissions.Scopes.Profile => OpenIdConnectScope.PROFILE,
            OpenIddictConstants.Permissions.Scopes.Roles => OpenIdConnectScope.ROLES,
            _ => throw new ArgumentOutOfRangeException(nameof(scope), $"Unsupported scope `{scope}`")
        };
    }

    public static string ToStringScope(this OpenIdConnectScope scope)
    {
        return scope switch
        {
            OpenIdConnectScope.READ_API => OpenIddictConstants.Permissions.Prefixes.Scope + AuthConfiguration.ReadApiScope,
            OpenIdConnectScope.WRITE_API => OpenIddictConstants.Permissions.Prefixes.Scope + AuthConfiguration.WriteApiScope,
            OpenIdConnectScope.MANAGE_USER_API => OpenIddictConstants.Permissions.Prefixes.Scope + AuthConfiguration.ManageUserApiScope,
            OpenIdConnectScope.ADDRESS => OpenIddictConstants.Permissions.Scopes.Address,
            OpenIdConnectScope.EMAIL => OpenIddictConstants.Permissions.Scopes.Email,
            OpenIdConnectScope.PHONE => OpenIddictConstants.Permissions.Scopes.Phone,
            OpenIdConnectScope.PROFILE => OpenIddictConstants.Permissions.Scopes.Profile,
            OpenIdConnectScope.ROLES => OpenIddictConstants.Permissions.Scopes.Roles,
            _ => throw new ArgumentOutOfRangeException(nameof(scope), $"Unsupported scope `{scope}`")
        };
    }
}