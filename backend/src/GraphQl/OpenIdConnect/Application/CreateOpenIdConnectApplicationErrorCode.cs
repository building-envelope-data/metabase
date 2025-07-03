using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.OpenIdConnect.Application;

[SuppressMessage("Naming", "CA1707")]
public enum CreateOpenIdConnectApplicationErrorCode
{
    UNKNOWN,
    UNAUTHORIZED,
    UNKNOWN_INSTITUTION,
    DUPLICATE_CLIENT_ID,
}