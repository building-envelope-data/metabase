using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

[SuppressMessage("Naming", "CA1707")]
public enum OpenIdConnectEndpoint
{
    AUTHORIZATION,
    PUSHED_AUTHORIZATION,
    DEVICE_AUTHORIZATION,
    INTROSPECTION,
    END_SESSION,
    REVOCATION,
    TOKEN,
}