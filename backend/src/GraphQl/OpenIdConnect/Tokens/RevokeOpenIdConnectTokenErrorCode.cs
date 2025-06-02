using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.OpenIdConnect.Tokens;

[SuppressMessage("Naming", "CA1707")]
public enum RevokeOpenIdConnectTokenErrorCode
{
    UNKNOWN,
    UNAUTHORIZED,
    UNKNOWN_TOKEN
}