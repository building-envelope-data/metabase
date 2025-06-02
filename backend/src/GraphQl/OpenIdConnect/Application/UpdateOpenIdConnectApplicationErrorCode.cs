using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.OpenIdConnect.Application;

[SuppressMessage("Naming", "CA1707")]
public enum UpdateOpenIdConnectApplicationErrorCode
{
    UNKNOWN,
    UNAUTHORIZED,
    UNKNOWN_APPLICATION
}