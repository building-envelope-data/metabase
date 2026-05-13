using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

[SuppressMessage("Naming", "CA1707")]
public enum CreateOpenIdConnectApplicationErrorCode
{
    UNKNOWN,
    UNAUTHORIZED,
    UNKNOWN_INSTITUTION,
    DUPLICATE_CLIENT_ID,
    ILLEGAL_CONSENT_TYPE,
}