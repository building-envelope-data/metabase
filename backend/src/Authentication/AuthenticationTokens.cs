using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Authentication;
using OpenIddict.Client.AspNetCore;

namespace Metabase.Authentication;

public sealed record AuthenticationTokens(
    string AccessToken,
    DateTimeOffset? AccessTokenExpirationDate,
    string? IdentityToken,
    string? RefreshToken
)
{
    internal const string AccessTokenName = OpenIddictClientAspNetCoreConstants.Tokens.BackchannelAccessToken;
    internal const string AccessTokenExpirationDateName = OpenIddictClientAspNetCoreConstants.Tokens.BackchannelAccessTokenExpirationDate;
    internal const string IdentityTokenName = OpenIddictClientAspNetCoreConstants.Tokens.BackchannelIdentityToken;
    internal const string RefreshTokenName = OpenIddictClientAspNetCoreConstants.Tokens.RefreshToken;

    public string? AccessTokenExpirationDateAsString => AccessTokenExpirationDate?.ToString(CultureInfo.InvariantCulture);
    private static DateTimeOffset? ParseAccessTokenExpirationDate(string? dateString) =>
        DateTimeOffset.TryParse(
            dateString,
            CultureInfo.InvariantCulture,
            out var date
        )
        ? date
        : null;

    public static string? ExtractAccessToken(AuthenticateResult result) =>
        result.Properties?.GetTokenValue(AccessTokenName);

    public static DateTimeOffset? ExtractAccessTokenExpirationDate(AuthenticateResult result) =>
        ParseAccessTokenExpirationDate(result.Properties?.GetTokenValue(AccessTokenExpirationDateName));

    public static string? ExtractIdentityToken(AuthenticateResult result) =>
        result.Properties?.GetTokenValue(IdentityTokenName);

    public static string? ExtractRefreshToken(AuthenticateResult result) =>
        result.Properties?.GetTokenValue(RefreshTokenName);

    public static AuthenticationTokens From(string accessToken, AuthenticateResult result)
    {
        return new(
            AccessToken: accessToken,
            AccessTokenExpirationDate: ExtractAccessTokenExpirationDate(result),
            IdentityToken: ExtractIdentityToken(result),
            RefreshToken: ExtractRefreshToken(result)
        );
    }
};