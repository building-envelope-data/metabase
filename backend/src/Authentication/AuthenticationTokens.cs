using System;

namespace Metabase.Authentication;

public sealed record AuthenticationTokens(
    string AccessToken,
    DateTimeOffset? AccessTokenExpirationDate,
    string? IdentityToken,
    string? RefreshToken
);