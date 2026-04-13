using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

[SuppressMessage("Naming", "CA1707")]
public enum OpenIdConnectRequirement
{
    PROOF_KEY_FOR_CODE_EXCHANGE,
    PUSHED_AUTHORIZATION_REQUESTS,
}