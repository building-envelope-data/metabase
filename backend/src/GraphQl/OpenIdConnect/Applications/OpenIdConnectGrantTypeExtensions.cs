using System;
using OpenIddict.Abstractions;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

public static class OpenIdConnectGrantTypeExtensions
{
    public static OpenIdConnectGrantType PermissionToOpenIdConnectGrantType(this string grantTypePermission)
    {
        return grantTypePermission switch
        {
            OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode => OpenIdConnectGrantType.AUTHORIZATION_CODE,
            OpenIddictConstants.Permissions.GrantTypes.ClientCredentials => OpenIdConnectGrantType.CLIENT_CREDENTIALS,
            OpenIddictConstants.Permissions.GrantTypes.RefreshToken => OpenIdConnectGrantType.REFRESH_TOKEN,
            OpenIddictConstants.Permissions.GrantTypes.TokenExchange => OpenIdConnectGrantType.TOKEN_EXCHANGE,
            _ => throw new ArgumentOutOfRangeException(nameof(grantTypePermission), $"Unsupported grant type `{grantTypePermission}`")
        };
    }

    public static string ToPermissionString(this OpenIdConnectGrantType grantType)
    {
        return grantType switch
        {
            OpenIdConnectGrantType.AUTHORIZATION_CODE => OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
            OpenIdConnectGrantType.CLIENT_CREDENTIALS => OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
            OpenIdConnectGrantType.REFRESH_TOKEN => OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
            OpenIdConnectGrantType.TOKEN_EXCHANGE => OpenIddictConstants.Permissions.GrantTypes.TokenExchange,
            _ => throw new ArgumentOutOfRangeException(nameof(grantType), $"Unsupported grant type `{grantType}`")
        };
    }
}