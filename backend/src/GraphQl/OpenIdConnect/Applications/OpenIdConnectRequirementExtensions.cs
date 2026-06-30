using System;
using System.Diagnostics.Contracts;
using OpenIddict.Abstractions;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

public static class OpenIdConnectRequirementExtensions
{
    [Pure]
    public static OpenIdConnectRequirement ToOpenIdConnectRequirement(this string requirement)
    {
        return requirement switch
        {
            OpenIddictConstants.Requirements.Features.ProofKeyForCodeExchange => OpenIdConnectRequirement.PROOF_KEY_FOR_CODE_EXCHANGE,
            OpenIddictConstants.Requirements.Features.PushedAuthorizationRequests => OpenIdConnectRequirement.PUSHED_AUTHORIZATION_REQUESTS,
            _ => throw new ArgumentOutOfRangeException(nameof(requirement), $"Unsupported requirement `{requirement}`")
        };
    }

    [Pure]
    public static string ToStringRequirement(this OpenIdConnectRequirement requirement)
    {
        return requirement switch
        {
            OpenIdConnectRequirement.PROOF_KEY_FOR_CODE_EXCHANGE => OpenIddictConstants.Requirements.Features.ProofKeyForCodeExchange,
            OpenIdConnectRequirement.PUSHED_AUTHORIZATION_REQUESTS => OpenIddictConstants.Requirements.Features.PushedAuthorizationRequests,
            _ => throw new ArgumentOutOfRangeException(nameof(requirement), $"Unsupported requirement `{requirement}`")
        };
    }
}