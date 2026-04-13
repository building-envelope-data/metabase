using System.Collections.Generic;

namespace Metabase.GraphQl.OpenIdConnect.Tokens;

public sealed class RevokeOpenIdConnectTokenError(
    RevokeOpenIdConnectTokenErrorCode code,
    string message,
    IReadOnlyList<string> path
    )
: UserErrorBase<RevokeOpenIdConnectTokenErrorCode>(code, message, path)
{
}