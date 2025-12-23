using System;
using OpenIddict.Abstractions;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

public static class OpenIdConnectConsentTypeExtensions
{
    public static OpenIdConnectConsentType ToOpenIdConnectConsentType(this string consentType)
    {
        return consentType switch
        {
            OpenIddictConstants.ConsentTypes.Explicit => OpenIdConnectConsentType.EXPLICIT,
            _ => throw new ArgumentOutOfRangeException(nameof(consentType), $"Unsupported consent type `{consentType}`")
        };
    }

    public static string ToStringConsentType(this OpenIdConnectConsentType consentType)
    {
        return consentType switch
        {
            OpenIdConnectConsentType.EXPLICIT => OpenIddictConstants.ConsentTypes.Explicit,
            _ => throw new ArgumentOutOfRangeException(nameof(consentType), $"Unsupported consent type `{consentType}`")
        };
    }
}