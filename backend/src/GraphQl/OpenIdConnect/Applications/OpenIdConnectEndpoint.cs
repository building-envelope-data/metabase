using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

[SuppressMessage("Naming", "CA1707")]
public enum OpenIdConnectEndpoint
{
    AUTHORIZATION,
    END_SESSION,
    INTROSPECTION,
    PUSHED_AUTHORIZATION,
    REVOCATION,
    TOKEN,
}