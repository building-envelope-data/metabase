using System;
using Metabase.Configuration;
using OpenIddict.Abstractions;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

public static class OpenIdConnectGrantTypeExtensions
{
    public static OpenIdConnectGrantType ToOpenIdConnectGrantType(this string grantType)
    {
        return grantType switch
        {
            OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode => OpenIdConnectGrantType.AUTHORIZATION_CODE,
            OpenIddictConstants.Permissions.GrantTypes.ClientCredentials => OpenIdConnectGrantType.CLIENT_CREDENTIALS,
            OpenIddictConstants.Permissions.GrantTypes.RefreshToken => OpenIdConnectGrantType.REFRESH_TOKEN,
            _ => throw new ArgumentOutOfRangeException(nameof(grantType), $"Unsupported grant type `{grantType}`")
        };
    }

    public static string ToStringGrantType(this OpenIdConnectGrantType grantType)
    {
        return grantType switch
        {
            OpenIdConnectGrantType.AUTHORIZATION_CODE => OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
            OpenIdConnectGrantType.CLIENT_CREDENTIALS => OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
            OpenIdConnectGrantType.REFRESH_TOKEN => OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
            _ => throw new ArgumentOutOfRangeException(nameof(grantType), $"Unsupported grant type `{grantType}`")
        };
    }
}