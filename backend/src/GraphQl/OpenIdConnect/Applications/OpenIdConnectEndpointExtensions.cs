using System;
using Metabase.Configuration;
using OpenIddict.Abstractions;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

public static class OpenIdConnectEndpointExtensions
{
    public static OpenIdConnectEndpoint ToOpenIdConnectEndpoint(this string endpoint)
    {
        return endpoint switch
        {
            OpenIddictConstants.Permissions.Endpoints.Authorization => OpenIdConnectEndpoint.AUTHORIZATION,
            OpenIddictConstants.Permissions.Endpoints.EndSession => OpenIdConnectEndpoint.END_SESSION,
            OpenIddictConstants.Permissions.Endpoints.Introspection => OpenIdConnectEndpoint.INTROSPECTION,
            OpenIddictConstants.Permissions.Endpoints.PushedAuthorization => OpenIdConnectEndpoint.PUSHED_AUTHORIZATION,
            OpenIddictConstants.Permissions.Endpoints.Revocation => OpenIdConnectEndpoint.REVOCATION,
            OpenIddictConstants.Permissions.Endpoints.Token => OpenIdConnectEndpoint.TOKEN,
            _ => throw new ArgumentOutOfRangeException(nameof(endpoint), $"Unsupported endpoint `{endpoint}`")
        };
    }

    public static string ToStringEndpoint(this OpenIdConnectEndpoint endpoint)
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