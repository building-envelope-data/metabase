using System;
using OpenIddict.Abstractions;

namespace Metabase.GraphQl.OpenIdConnect.Application;

public static class OpenIdConnectConsentTypeExtensions
{
    public static OpenIdConnectConsentType ToOpenIdConnectConsentType(this string consentType)
    {
        return consentType switch
        {
            OpenIddictConstants.ConsentTypes.Explicit => OpenIdConnectConsentType.EXPLICIT,
            OpenIddictConstants.ConsentTypes.External => OpenIdConnectConsentType.EXTERNAL,
            OpenIddictConstants.ConsentTypes.Implicit => OpenIdConnectConsentType.IMPLICIT,
            OpenIddictConstants.ConsentTypes.Systematic => OpenIdConnectConsentType.SYSTEMATIC,
            _ => throw new ArgumentOutOfRangeException(nameof(consentType), $"Unsupported consent type `{consentType}`")
        };
    }

    public static string ToStringConsentType(this OpenIdConnectConsentType consentType)
    {
        return consentType switch
        {
            OpenIdConnectConsentType.EXPLICIT => OpenIddictConstants.ConsentTypes.Explicit,
            OpenIdConnectConsentType.EXTERNAL => OpenIddictConstants.ConsentTypes.External,
            OpenIdConnectConsentType.IMPLICIT => OpenIddictConstants.ConsentTypes.Implicit,
            OpenIdConnectConsentType.SYSTEMATIC => OpenIddictConstants.ConsentTypes.Systematic,
            _ => throw new ArgumentOutOfRangeException(nameof(consentType), $"Unsupported consent type `{consentType}`")
        };
    }
}