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
using NodaTime;
using OpenIddict.Abstractions;
using OpenIddict.Client;
using OpenIddict.Client.AspNetCore;
using OpenIddict.Validation.AspNetCore;

namespace Metabase.Authentication;

public static partial class Log
{
    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Failed to set authentication tokens. {Errors}")]
    public static partial void FailedToSetAuthenticationTokens(
        this ILogger<AuthenticationHandler> logger,
        string Errors
    );

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Missing provider name.")]
    public static partial void MissingProviderName(
        this ILogger<AuthenticationHandler> logger
    );

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Missing user ID.")]
    public static partial void MissingUserId(
        this ILogger<AuthenticationHandler> logger
    );

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Unknown user with ID '{UserId}'")]
    public static partial void UnknownUser(
        this ILogger<AuthenticationHandler> logger,
        string userId
    );
}

public sealed class AuthenticationHandler(
    IClock clock,
    UserManager<User> userManager,
    OpenIddictClientService openIddictClientService,
    ILogger<AuthenticationHandler> logger
)
{
    private const string DateTimeOffsetFormat = "o";

    private async Task<IdentityResult> SetAccessTokenExpirationDateAsync(
        User user,
        string providerName,
        DateTimeOffset? accessTokenExpirationDate
    ) =>
        await userManager.SetAuthenticationTokenAsync(
            user,
            providerName,
            AuthenticationTokens.AccessTokenExpirationDateName,
            accessTokenExpirationDate?.ToString(DateTimeOffsetFormat, CultureInfo.InvariantCulture)
        );

    private async Task<DateTimeOffset?> GetAccessTokenExpirationDateAsync(
        User user,
        string providerName
    ) =>
        DateTimeOffset.TryParseExact(
            await userManager.GetAuthenticationTokenAsync(
                user,
                providerName,
                AuthenticationTokens.AccessTokenExpirationDateName
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
        string providerName,
        AuthenticationTokens freshTokens
    )
    {
        var identityResults = new List<(string Token, IdentityResult Result)>
        {
            (
                AuthenticationTokens.AccessTokenName,
                await userManager.SetAuthenticationTokenAsync(
                    user,
                    providerName,
                    AuthenticationTokens.AccessTokenName,
                    freshTokens.AccessToken
                )
            ),
            (
                AuthenticationTokens.AccessTokenExpirationDateName,
                await SetAccessTokenExpirationDateAsync(
                    user,
                    providerName,
                    freshTokens.AccessTokenExpirationDate
                )
            ),
            (
                AuthenticationTokens.IdentityTokenName,
                await userManager.SetAuthenticationTokenAsync(
                    user,
                    providerName,
                    AuthenticationTokens.IdentityTokenName,
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
                        providerName,
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
        var providerName = cookieAuthenticationResult.Principal.GetClaim(OpenIddictConstants.Claims.Private.ProviderName);
        if (providerName is null)
        {
            logger.MissingProviderName();
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
            providerName,
            AuthenticationTokens.AccessTokenName
        );
        var expirationDate = await GetAccessTokenExpirationDateAsync(user, providerName);
        if (accessToken is not null
            && expirationDate is not null
            && clock.GetUtcNow().ToDateTimeOffset() <= expirationDate?.Subtract(OpenIdConnectConstants.AccessAndIdentityTokenLifetime.Divide(3))
        )
        {
            return accessToken;
        }
        var refreshToken = await userManager.GetAuthenticationTokenAsync(
            user,
            providerName,
            OpenIddictClientAspNetCoreConstants.Tokens.RefreshToken
        );
        if (refreshToken is not null)
        {
            var refreshTokenAuthenticationResult = await openIddictClientService.AuthenticateWithRefreshTokenAsync(
                new OpenIddictClientModels.RefreshTokenAuthenticationRequest
                {
                    DisableUserInfo = true,
                    RefreshToken = refreshToken,
                    CancellationToken = cancellationToken,
                }
            );
            var errors = await SetAuthenticationTokensAsync(
                user,
                providerName,
                new AuthenticationTokens(
                    AccessToken: refreshTokenAuthenticationResult.AccessToken,
                    AccessTokenExpirationDate: refreshTokenAuthenticationResult.AccessTokenExpirationDate,
                    IdentityToken: refreshTokenAuthenticationResult.IdentityToken,
                    RefreshToken: refreshTokenAuthenticationResult.RefreshToken
                )
            );
            if (errors.Count > 0)
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
