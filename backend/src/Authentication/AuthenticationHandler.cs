using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Data;
using Metabase.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;
using OpenIddict.Client;
using OpenIddict.Client.AspNetCore;
using OpenIddict.Validation.AspNetCore;
using static OpenIddict.Client.OpenIddictClientModels;

namespace Metabase.Authentication;

public static partial class Log
{
    [LoggerMessage(
        EventId = 0,
        Level = LogLevel.Error,
        Message = "Failed to set authentication tokens. {Errors}")]
    public static partial void FailedToSetAuthenticationTokens(
        this ILogger<AuthenticationHandler> logger,
        string Errors
    );

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Information,
        Message = "Missing user ID.")]
    public static partial void MissingUserId(
        this ILogger<AuthenticationHandler> logger
    );

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Information,
        Message = "Unknown user with ID '{UserId}'")]
    public static partial void UnknownUser(
        this ILogger<AuthenticationHandler> logger,
        string userId
    );
}

public sealed class AuthenticationHandler(
    UserManager<User> userManager,
    OpenIddictClientService openIddictClientService,
    ILogger<AuthenticationHandler> logger
)
{
    private const string LoginProvider = OpenIdConnectConstants.MetabaseClientId;
    private const string DateTimeOffsetFormat = "o";

    private async Task<IdentityResult> SetBackchannelAccessTokenExpirationDateAsync(
        User user,
        DateTimeOffset? accessTokenExpirationDate
    ) =>
        await userManager.SetAuthenticationTokenAsync(
            user,
            LoginProvider,
            OpenIddictClientAspNetCoreConstants.Tokens.BackchannelAccessTokenExpirationDate,
            accessTokenExpirationDate?.ToString(DateTimeOffsetFormat, CultureInfo.InvariantCulture)
        );

    private async Task<DateTimeOffset?> GetBackchannelAccessTokenExpirationDateAsync(
        User user
    ) =>
        DateTimeOffset.TryParseExact(
            await userManager.GetAuthenticationTokenAsync(
                user,
                LoginProvider,
                OpenIddictClientAspNetCoreConstants.Tokens.BackchannelAccessTokenExpirationDate
            ),
            DateTimeOffsetFormat,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out var date
        )
        ? date
        : null;

    public async Task<IDictionary<string, IEnumerable<IdentityError>>> SetAuthenticationTokensAsync(
        User user,
        AuthenticationTokens freshTokens
    )
    {
        var identityResults = new List<(string Token, IdentityResult Result)>
        {
            (
                OpenIddictClientAspNetCoreConstants.Tokens.BackchannelAccessToken,
                await userManager.SetAuthenticationTokenAsync(
                    user,
                    LoginProvider,
                    OpenIddictClientAspNetCoreConstants.Tokens.BackchannelAccessToken,
                    freshTokens.AccessToken
                )
            ),
            (
                OpenIddictClientAspNetCoreConstants.Tokens.BackchannelAccessTokenExpirationDate,
                await SetBackchannelAccessTokenExpirationDateAsync(
                    user,
                    freshTokens.AccessTokenExpirationDate
                )
            ),
            (
                OpenIddictClientAspNetCoreConstants.Tokens.BackchannelIdentityToken,
                await userManager.SetAuthenticationTokenAsync(
                    user,
                    LoginProvider,
                    OpenIddictClientAspNetCoreConstants.Tokens.BackchannelIdentityToken,
                    freshTokens.IdentityToken
                )
            ),
        };
        if (freshTokens.RefreshToken is not null)
        {
            identityResults.Add(
                (
                    OpenIddictClientAspNetCoreConstants.Tokens.RefreshToken,
                    await userManager.SetAuthenticationTokenAsync(
                        user,
                        LoginProvider,
                        OpenIddictClientAspNetCoreConstants.Tokens.RefreshToken,
                        freshTokens.RefreshToken
                    )
                )
            );
        }
        return identityResults
            .Where(_ => _.Result is not { Succeeded: true })
            .ToImmutableDictionary(
                _ => _.Token,
                _ => _.Result.Errors
            );
    }

    private async Task<string?> FetchAndRefreshAccessTokenFromCookieAuthenticationAsync(
        HttpContext httpContext,
        CancellationToken cancellationToken
    )
    {
        var cookieAuthenticationResult = await httpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        if (cookieAuthenticationResult is not { Succeeded: true, Principal.Identity.IsAuthenticated: true })
        {
            return null;
        }
        var userId = cookieAuthenticationResult.Principal.GetClaim(ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            logger.MissingUserId();
            return null;
        }
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            logger.UnknownUser(userId);
            return null;
        }
        var accessToken = await userManager.GetAuthenticationTokenAsync(
            user,
            LoginProvider,
            OpenIddictClientAspNetCoreConstants.Tokens.BackchannelAccessToken
        );
        var expirationDate = await GetBackchannelAccessTokenExpirationDateAsync(user);
        if (accessToken is not null
            && expirationDate is not null
            && TimeProvider.System.GetUtcNow() <= expirationDate?.Subtract(OpenIdConnectConstants.AccessAndIdentityTokenLifetime.Divide(3))
        )
        {
            return accessToken;
        }
        var refreshToken = await userManager.GetAuthenticationTokenAsync(
            user,
            LoginProvider,
            OpenIddictClientAspNetCoreConstants.Tokens.RefreshToken
        );
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
            var errors = await SetAuthenticationTokensAsync(
                user,
                new AuthenticationTokens(
                    AccessToken: refreshTokenAuthenticationResult.AccessToken,
                    AccessTokenExpirationDate: refreshTokenAuthenticationResult.AccessTokenExpirationDate,
                    IdentityToken: refreshTokenAuthenticationResult.IdentityToken,
                    RefreshToken: refreshTokenAuthenticationResult.RefreshToken
                )
            );
            if (errors.Count >= 1)
            {
                logger.FailedToSetAuthenticationTokens(
                    string.Join(
                        " ",
                        errors.Select(_ => $"Could not store the authentication token '{_.Key}': {string.Join(", ", _.Value.Select(_ => $"* [{_.Code}] '{_.Description}'"))}.")
                    )
                );
            }
            return refreshTokenAuthenticationResult.AccessToken;
        }
        return null;
    }

    public async Task<AuthenticateResult> AuthenticateAsync(
        HttpContext httpContext,
        CancellationToken cancellationToken
    )
    {
        if (AuthenticationHelpers.IsSameOriginOrReferer(httpContext.Request))
        {
            if (AuthenticationHelpers.IsReferredToFromSubpath(httpContext.Request, AuthenticationConstants.LoginPath))
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
                return await httpContext.AuthenticateAsync(AuthenticationConstants.IdentityApplicationScheme);
            }
            // For the Next.js Web frontend, the metabase acts as OpenId Connect
            // Client and uses the cookie scheme to store access, identity, and
            // refresh tokens. We extract the access token from the cookie, refresh
            // it if needed, and set it as `Bearer` token of the HTTP authorization
            // header. This token is used below by the JWT authentication schema.
            // For the configuration of the "cookie scheme" cookie, see
            // `AuthConfiguration#ConfigureAuthenticationAndAuthorizationServices`
            // This cookie is set in `AuthenticationController` whose actions are
            // used by the sign-in process of `OpenIddictBuilder#AddClient` in
            // `AuthConfiguration#ConfigureOpenIddictServices`.
            var accessToken = await FetchAndRefreshAccessTokenFromCookieAuthenticationAsync(
                httpContext,
                cancellationToken
            );
            if (accessToken is not null)
            {
                httpContext.SetBearerToken(accessToken);
            }
        }
        // For third-party frontends, the metabase acts as resource server
        // and uses authorization-header bearer tokens for authentication,
        // that is JavaScript Web Tokens (JWT), aka, Access Tokens, provided
        // as `Authorization` HTTP header with the prefix `Bearer` as issued
        // by OpenIddict. This Access Token includes Scopes and Claims. The
        // scheme is configured in
        // `AuthConfiguration#ConfigureOpenIddictServices` by
        // `OpenIddictBuilder#AddValidation`.
        return await httpContext.AuthenticateAsync(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
    }
}