using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

[SuppressMessage("Naming", "CA1707")]
public enum UpdateOpenIdConnectApplicationErrorCode
{
    UNKNOWN,
    UNAUTHORIZED,
    UNKNOWN_APPLICATION,
    ILLEGAL_CONSENT_TYPE
}