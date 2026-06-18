using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using OpenIddict.Abstractions;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

public static class OpenIdConnectGrantTypeExtensions
{
    [Pure]
    public static OpenIdConnectGrantType[] PermissionsToOpenIdConnectGrantTypes(this List<string> permissions)
    {
        return permissions.FindAll(permission =>
        {
            try
            {
                var ignore = permission.PermissionToOpenIdConnectGrantType();
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        })
        .Select(grantTypePermission => grantTypePermission.PermissionToOpenIdConnectGrantType())
        .ToArray();
    }

    [Pure]
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

    [Pure]
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