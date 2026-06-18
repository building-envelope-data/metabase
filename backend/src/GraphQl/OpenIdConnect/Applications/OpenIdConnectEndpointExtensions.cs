using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using OpenIddict.Abstractions;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

public static class OpenIdConnectEndpointExtensions
{
    [Pure]
    public static OpenIdConnectEndpoint[] PermissionsToOpenIdConnectEndpoints(this List<string> permissions)
    {
        return permissions.FindAll(permission =>
        {
            try
            {
                var ignore = permission.PermissionToOpenIdConnectEndpoint();
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        })
        .Select(endpointPermission => endpointPermission.PermissionToOpenIdConnectEndpoint())
        .ToArray();
    }

    [Pure]
    public static OpenIdConnectEndpoint PermissionToOpenIdConnectEndpoint(this string endpointPermission)
    {
        return endpointPermission switch
        {
            OpenIddictConstants.Permissions.Endpoints.Authorization => OpenIdConnectEndpoint.AUTHORIZATION,
            OpenIddictConstants.Permissions.Endpoints.EndSession => OpenIdConnectEndpoint.END_SESSION,
            OpenIddictConstants.Permissions.Endpoints.Introspection => OpenIdConnectEndpoint.INTROSPECTION,
            OpenIddictConstants.Permissions.Endpoints.PushedAuthorization => OpenIdConnectEndpoint.PUSHED_AUTHORIZATION,
            OpenIddictConstants.Permissions.Endpoints.Revocation => OpenIdConnectEndpoint.REVOCATION,
            OpenIddictConstants.Permissions.Endpoints.Token => OpenIdConnectEndpoint.TOKEN,
            _ => throw new ArgumentOutOfRangeException(nameof(endpointPermission), $"Unsupported endpoint `{endpointPermission}`")
        };
    }

    [Pure]
    public static string ToPermissionString(this OpenIdConnectEndpoint endpoint)
    {
        return endpoint switch
        {
            OpenIdConnectEndpoint.AUTHORIZATION => OpenIddictConstants.Permissions.Endpoints.Authorization,
            OpenIdConnectEndpoint.END_SESSION => OpenIddictConstants.Permissions.Endpoints.EndSession,
            OpenIdConnectEndpoint.INTROSPECTION => OpenIddictConstants.Permissions.Endpoints.Introspection,
            OpenIdConnectEndpoint.PUSHED_AUTHORIZATION => OpenIddictConstants.Permissions.Endpoints.PushedAuthorization,
            OpenIdConnectEndpoint.REVOCATION => OpenIddictConstants.Permissions.Endpoints.Revocation,
            OpenIdConnectEndpoint.TOKEN => OpenIddictConstants.Permissions.Endpoints.Token,
            _ => throw new ArgumentOutOfRangeException(nameof(endpoint), $"Unsupported endpoint `{endpoint}`")
        };
    }
}