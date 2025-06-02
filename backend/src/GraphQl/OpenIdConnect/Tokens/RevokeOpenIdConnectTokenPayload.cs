using System.Collections.Generic;

namespace Metabase.GraphQl.OpenIdConnect.Tokens;

public sealed class RevokeOpenIdConnectTokenPayload
{
    public RevokeOpenIdConnectTokenPayload()
    {
    }

    public RevokeOpenIdConnectTokenPayload(
        RevokeOpenIdConnectTokenError error
    )
    {
        Errors = [error];
    }

    public IReadOnlyCollection<RevokeOpenIdConnectTokenError>? Errors { get; }
}