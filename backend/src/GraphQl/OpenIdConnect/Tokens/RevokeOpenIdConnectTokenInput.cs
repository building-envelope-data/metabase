using System;

namespace Metabase.GraphQl.OpenIdConnect.Tokens;

public sealed record RevokeOpenIdConnectTokenInput(
    Guid TokenId
);