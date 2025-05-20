using System.Collections.Generic;

namespace Metabase.GraphQl.OpenIdConnect.Tokens;

public sealed class RevokeTokenPayload
{
    public RevokeTokenPayload()
    {
    }

    public RevokeTokenPayload(
        RevokeTokenError error
    )
    {
        Errors = [error];
    }

    public IReadOnlyCollection<RevokeTokenError>? Errors { get; }
}