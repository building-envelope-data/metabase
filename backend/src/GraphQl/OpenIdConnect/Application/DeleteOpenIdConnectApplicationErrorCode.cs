using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.OpenIdConnect.Application;

[SuppressMessage("Naming", "CA1707")]
public enum DeleteOpenIdConnectApplicationErrorCode
{
    UNKNOWN,
    UNAUTHORIZED,
    UNKNOWN_APPLICATION
}