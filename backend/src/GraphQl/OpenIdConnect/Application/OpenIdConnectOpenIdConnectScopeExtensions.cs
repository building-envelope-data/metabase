using System;
using Metabase.Configuration;
using OpenIddict.Abstractions;

namespace Metabase.GraphQl.OpenIdConnect.Application;

public static class OpenIdConnectScopeExtensions
{
    public static OpenIdConnectScope ToOpenIdConnectScope(this string scope)
    {
        return scope switch
        {
            OpenIddictConstants.Permissions.Prefixes.Scope + AuthConfiguration.ReadApiScope => OpenIdConnectScope.READ_API,
            OpenIddictConstants.Permissions.Prefixes.Scope + AuthConfiguration.WriteApiScope => OpenIdConnectScope.WRITE_API,
            OpenIddictConstants.Permissions.Prefixes.Scope + AuthConfiguration.ManageUserApiScope => OpenIdConnectScope.MANAGE_USER_API,
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
            _ => throw new ArgumentOutOfRangeException(nameof(scope), $"Unsupported scope `{scope}`")
        };
    }
}