using System;
using Metabase.Configuration;
using OpenIddict.Abstractions;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

public static class OpenIdConnectResponseTypeExtensions
{
    public static OpenIdConnectResponseType ToOpenIdConnectResponseType(this string responseType)
    {
        return responseType switch
        {
            OpenIddictConstants.Permissions.ResponseTypes.Code => OpenIdConnectResponseType.CODE,
            OpenIddictConstants.Permissions.ResponseTypes.Token => OpenIdConnectResponseType.TOKEN,
            _ => throw new ArgumentOutOfRangeException(nameof(responseType), $"Unsupported response type `{responseType}`")
        };
    }

    public static string ToStringResponseType(this OpenIdConnectResponseType responseType)
    {
        return responseType switch
        {
            OpenIdConnectResponseType.CODE => OpenIddictConstants.Permissions.ResponseTypes.Code,
            OpenIdConnectResponseType.TOKEN => OpenIddictConstants.Permissions.ResponseTypes.Token,
            _ => throw new ArgumentOutOfRangeException(nameof(responseType), $"Unsupported response type `{responseType}`")
        };
    }
}