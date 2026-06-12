using System;
using System.Diagnostics.Contracts;
using OpenIddict.Abstractions;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

public static class OpenIdConnectResponseTypeExtensions
{
    [Pure]
    public static OpenIdConnectResponseType PermissionToOpenIdConnectResponseType(this string responseTypePermission)
    {
        return responseTypePermission switch
        {
            OpenIddictConstants.Permissions.ResponseTypes.Code => OpenIdConnectResponseType.CODE,
            OpenIddictConstants.Permissions.ResponseTypes.IdToken => OpenIdConnectResponseType.ID_TOKEN,
            OpenIddictConstants.Permissions.ResponseTypes.Token => OpenIdConnectResponseType.TOKEN,
            _ => throw new ArgumentOutOfRangeException(nameof(responseTypePermission), $"Unsupported response type `{responseTypePermission}`")
        };
    }

    [Pure]
    public static string ToPermissionString(this OpenIdConnectResponseType responseType)
    {
        return responseType switch
        {
            OpenIdConnectResponseType.CODE => OpenIddictConstants.Permissions.ResponseTypes.Code,
            OpenIdConnectResponseType.ID_TOKEN => OpenIddictConstants.Permissions.ResponseTypes.IdToken,
            OpenIdConnectResponseType.TOKEN => OpenIddictConstants.Permissions.ResponseTypes.Token,
            _ => throw new ArgumentOutOfRangeException(nameof(responseType), $"Unsupported response type `{responseType}`")
        };
    }
}