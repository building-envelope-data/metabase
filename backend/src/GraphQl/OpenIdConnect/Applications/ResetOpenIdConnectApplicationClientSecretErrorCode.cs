using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

[SuppressMessage("Naming", "CA1707")]
public enum ResetOpenIdConnectApplicationClientSecretErrorCode
{
    UNKNOWN,
    UNAUTHORIZED,
    UNKNOWN_APPLICATION
}