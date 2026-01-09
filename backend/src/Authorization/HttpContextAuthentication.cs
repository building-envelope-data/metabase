using System;
using System.Globalization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using OpenIddict.Abstractions;
using OpenIddict.Client;
using OpenIddict.Client.AspNetCore;
using OpenIddict.Validation.AspNetCore;
using static OpenIddict.Client.OpenIddictClientModels;

namespace Metabase.Authorization;

public static class HttpContextAuthentication
{
    public static async Task<AuthenticateResult> AuthenticateAsync(
        HttpContext httpContext,
        OpenIddictClientService openIddictClientService,
        CancellationToken cancellationToken
    )
    {
        // For the login part of the Web frontend, the metabase acts as
        // OpenId Connect Authorization Server and uses the identity
        // application cookie scheme for authentication between the user
        // signing-in and accepting or denying the clients request for
        // certain scopes on behalf of the user. See
        // `AuthConfiguration#ConfigureIdentityServices` for the configuration
        // of the identity application cookie. And, for the cookie's usage
        // in the authorization code flow, see
        // `AuthorizationController#Authorize`,
        // `AuthorizationController#Accept` `AuthorizationController#Deny`.
        // TODO Only use explicitly in the respective antiforgery endpoint and/or GraphQL mutations/queries.
        var identityAuthenticateResult = await httpContext.AuthenticateAsync(AuthConfiguration.IdentityConstantsApplicationScheme);
        if (identityAuthenticateResult is { Succeeded: true, Principal.Identity.IsAuthenticated: true })
        {
            httpContext.User = identityAuthenticateResult.Principal;
            return identityAuthenticateResult;
        }

        // For the Next.js Web frontend, the metabase acts as OpenId Connect
        // Client and uses the cookie scheme to store access and refresh tokens. See
        // `AuthConfiguration#ConfigureAuthenticationAndAuthorizationServices`
        // for the configuration of the "cookie scheme" cookie. This cookie
        // is set by methods in `AuthenticationController` and is related to
        // `OpenIddictBuilder#AddClient` in
        // `AuthConfiguration#ConfigureOpenIddictServices`.
        var newTokens = await SetAndRefreshBearerTokenFromCookieAuthenticationAsync(
            httpContext,
            openIddictClientService,
            cancellationToken
        );

        // For third-party frontends, the metabase acts as resource server
        // and uses authorization-header bearer tokens for authentication,
        // that is JavaScript Web Tokens (JWT), aka, Access Tokens, provided
        // as `Authorization` HTTP header with the prefix `Bearer` as issued
        // by OpenIddict. This Access Token includes Scopes and Claims. The
        // scheme is configured in
        // `AuthConfiguration#ConfigureOpenIddictServices` by
        // `OpenIddictBuilder#AddValidation`.
        var jwtAuthenticateResult = await httpContext.AuthenticateAsync(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        if (jwtAuthenticateResult is { Succeeded: true, Principal.Identity.IsAuthenticated: true })
        {
            httpContext.User = jwtAuthenticateResult.Principal;
            if (newTokens is not null)
            {
                await UpdateTokensInCookieAsync(httpContext, jwtAuthenticateResult.Principal, jwtAuthenticateResult.Properties, newTokens);
            }
            return jwtAuthenticateResult;
        }

        return AuthenticateResult.Fail("All available authentication schemes failed or yielded no claims principal.");
    }

    private sealed record NewTokens(
        string AccessToken,
        DateTimeOffset? AccessTokenExpirationDate,
        string? IdentityToken,
        string? RefreshToken
    );

    private static string? GetBackchannelAccessToken(AuthenticateResult authenticateResult) =>
        authenticateResult.Properties?.GetTokenValue(OpenIddictClientAspNetCoreConstants.Tokens.BackchannelAccessToken);

    private static DateTimeOffset? GetBackchannelAccessTokenExpirationDate(AuthenticateResult authenticateResult) =>
        DateTimeOffset.TryParse(
            authenticateResult.Properties?.GetTokenValue(OpenIddictClientAspNetCoreConstants.Tokens.BackchannelAccessTokenExpirationDate),
            CultureInfo.InvariantCulture,
            out var date
        )
        ? date
        : null;

    private static string? GetRefreshToken(AuthenticateResult authenticateResult) =>
        authenticateResult.Properties?.GetTokenValue(OpenIddictClientAspNetCoreConstants.Tokens.RefreshToken);

    private static void SetBearerToken(HttpContext httpContext, string accessToken) =>
        httpContext.Request.Headers.Authorization = $"{OpenIddictConstants.Schemes.Bearer} {accessToken}";

    private static async Task<NewTokens?> SetAndRefreshBearerTokenFromCookieAuthenticationAsync(
        HttpContext httpContext,
        OpenIddictClientService openIddictClientService,
        CancellationToken cancellationToken
    )
    {
        var cookieAuthenticationResult = await httpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        if (cookieAuthenticationResult is not { Succeeded: true, Principal.Identity.IsAuthenticated: true })
        {
            return null;
        }
        var accessToken = GetBackchannelAccessToken(cookieAuthenticationResult);
        var expirationDate = GetBackchannelAccessTokenExpirationDate(cookieAuthenticationResult);
        if (accessToken is not null
            && expirationDate is not null
            && TimeProvider.System.GetUtcNow() <= expirationDate?.AddMinutes(-20)
        )
        {
            SetBearerToken(httpContext, accessToken);
            return null;
        }
        var refreshToken = GetRefreshToken(cookieAuthenticationResult);
        if (refreshToken is not null)
        {
            var refreshTokenAuthenticationResult = await openIddictClientService.AuthenticateWithRefreshTokenAsync(
                new RefreshTokenAuthenticationRequest
                {
                    DisableUserInfo = true,
                    RefreshToken = refreshToken,
                    CancellationToken = cancellationToken,
                }
            );
            SetBearerToken(httpContext, refreshTokenAuthenticationResult.AccessToken);
            return new NewTokens(
                AccessToken: refreshTokenAuthenticationResult.AccessToken,
                AccessTokenExpirationDate: refreshTokenAuthenticationResult.AccessTokenExpirationDate,
                IdentityToken: refreshTokenAuthenticationResult.IdentityToken,
                RefreshToken: refreshTokenAuthenticationResult.RefreshToken
            );
        }
        return null;
    }

    private static async Task UpdateTokensInCookieAsync(
        HttpContext httpContext,
        ClaimsPrincipal claimsPrincipal,
        AuthenticationProperties? authenticationProperties,
        NewTokens newTokens
    )
    {
        // Override the tokens using the values returned in the token response.
        var properties = authenticationProperties?.Clone() ?? new AuthenticationProperties();
        // Keep in sync with `GetTokenValue` in `AddRequestTransform` above
        properties.UpdateTokenValue(
            OpenIddictClientAspNetCoreConstants.Tokens.BackchannelAccessToken,
            newTokens.AccessToken
        );
        properties.UpdateTokenValue(
            OpenIddictClientAspNetCoreConstants.Tokens.BackchannelAccessTokenExpirationDate,
            newTokens.AccessTokenExpirationDate?.ToString(CultureInfo.InvariantCulture) ?? string.Empty
        );
        // Note: if no identity token was returned, preserve the identity token initially returned.
        if (!string.IsNullOrEmpty(newTokens.IdentityToken))
        {
            properties.UpdateTokenValue(
                OpenIddictClientAspNetCoreConstants.Tokens.BackchannelIdentityToken,
                newTokens.IdentityToken
            );
        }
        // Note: if no refresh token was returned, preserve the refresh token initially returned.
        if (!string.IsNullOrEmpty(newTokens.RefreshToken))
        {
            properties.UpdateTokenValue(
                OpenIddictClientAspNetCoreConstants.Tokens.RefreshToken,
                newTokens.RefreshToken
            );
        }
        // Remove the redirect URI from the authentication properties
        // to prevent the cookies handler from genering a 302 response.
        properties.RedirectUri = null;
        // Set the creation and expiration dates of the ticket to null to decorrelate the lifetime
        // of the resulting authentication cookie from the lifetime of the identity token returned by
        // the authorization server (if applicable). In this case, the expiration date time will be
        // automatically computed by the cookie handler using the lifetime configured in the options.
        //
        // Applications that prefer binding the lifetime of the ticket stored in the authentication cookie
        // to the identity token returned by the identity provider can remove or comment these two lines:
        properties.IssuedUtc = null;
        properties.ExpiresUtc = null;
        // Note: this flag controls whether the authentication cookie that will be returned to the
        // browser will be treated as a session cookie (i.e destroyed when the browser is closed)
        // or as a persistent cookie. In both cases, the lifetime of the authentication ticket is
        // always stored as protected data, preventing malicious users from trying to use an
        // authentication cookie beyond the lifetime of the authentication ticket itself.
        properties.IsPersistent = false;
        // If multiple HTTP responses for the same user are returned in parallel, the browser will
        // always store the latest cookie received and the refresh tokens stored in the other cookies
        // will be discarded.
        await httpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            claimsPrincipal,
            properties
        );
    }
}