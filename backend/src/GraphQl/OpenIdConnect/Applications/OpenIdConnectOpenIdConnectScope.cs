using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

[SuppressMessage("Naming", "CA1707")]
public enum OpenIdConnectScope
{
    READ_API,
    WRITE_API,
    MANAGE_USER_API,
}