using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.OpenIdConnect.Authorizations;

[SuppressMessage("Naming", "CA1707")]
public enum DeleteOpenIdConnectAuthorizationErrorCode
{
    UNKNOWN,
    UNAUTHORIZED,
    UNKNOWN_AUTHORIZATION
}