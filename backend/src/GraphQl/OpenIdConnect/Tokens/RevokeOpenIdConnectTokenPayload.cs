using System.Collections.Generic;
using Metabase.Data.OpenIdConnect;

namespace Metabase.GraphQl.OpenIdConnect.Tokens;

public sealed class RevokeOpenIdConnectTokenPayload
{
    public RevokeOpenIdConnectTokenPayload(OpenIdConnectToken token)
    {
        Token = token;
    }

    public RevokeOpenIdConnectTokenPayload(
        RevokeOpenIdConnectTokenError error
    )
    {
        Errors = [error];
    }

    public OpenIdConnectToken? Token { get; }
    public IReadOnlyCollection<RevokeOpenIdConnectTokenError>? Errors { get; }
}