using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using OpenIddict.Client;
using static OpenIddict.Abstractions.OpenIddictConstants;
using static OpenIddict.Client.AspNetCore.OpenIddictClientAspNetCoreConstants;
using static OpenIddict.Client.OpenIddictClientModels;

namespace Metabase.Helpers;

// Inspired by https://github.com/openiddict/openiddict-samples/blob/dev/samples/Dantooine/Dantooine.WebAssembly.Server/Helpers/TokenRefreshingDelegatingHandler.cs
internal sealed class TokenRefreshingDelegatingHandler(
    OpenIddictClientService service,
    HttpMessageHandler innerHandler
)
: DelegatingHandler(innerHandler)
{
    private static string GetBackchannelAccessToken(HttpRequestOptions options) =>
        options.TryGetValue(new(Tokens.BackchannelAccessToken), out string? token)
        && !string.IsNullOrEmpty(token)
        ? token
        : throw new InvalidOperationException("The access token couldn't be found in the request options.");

    private static DateTimeOffset? GetBackchannelAccessTokenExpirationDate(HttpRequestOptions options) =>
        options.TryGetValue(new(Tokens.BackchannelAccessTokenExpirationDate), out string? token)
        && !string.IsNullOrEmpty(token)
        && DateTimeOffset.TryParse(token, CultureInfo.InvariantCulture, out var date)
        ? date
        : null;

    private static string GetRefreshToken(HttpRequestOptions options) =>
        options.TryGetValue(new(Tokens.RefreshToken), out string? token)
        && !string.IsNullOrEmpty(token)
        ? token
        : throw new InvalidOperationException("The refresh token couldn't be found in the request options.");

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        // If an access token expiration date was returned by the authorization server and stored
        // in the authentication cookie, use it to determine whether the token is about to expire.
        // If it's not, try to use it: if the resource server returns a 401 error response, try
        // to refresh the tokens before replaying the request with the new access token attached.
        // Otherwise, don't bother using the existing access token and refresh tokens immediately.
        // Note: this handler can be called concurrently for the same user if multiple HTTP
        // requests are processed in parallel: while this results in multiple refresh token
        // requests being sent concurrently, this is something OpenIddict allows during a short
        // period of time (called refresh token reuse leeway and set to 30 seconds by default).
        var expirationDate = GetBackchannelAccessTokenExpirationDate(request.Options);
        if (expirationDate is null || TimeProvider.System.GetUtcNow() <= expirationDate?.AddMinutes(-5))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue(
                Schemes.Bearer,
                GetBackchannelAccessToken(request.Options)
            );
            var response = await base.SendAsync(request, cancellationToken);
            if (response.StatusCode is not HttpStatusCode.Unauthorized)
            {
                return response;
            }
        }
        var result = await service.AuthenticateWithRefreshTokenAsync(
            new RefreshTokenAuthenticationRequest
            {
                CancellationToken = cancellationToken,
                DisableUserInfo = true,
                RefreshToken = GetRefreshToken(request.Options)
            }
        );
        request.Headers.Authorization = new AuthenticationHeaderValue(
            Schemes.Bearer,
            result.AccessToken
        );
        return new TokenRefreshingHttpResponseMessage(
            result,
            await base.SendAsync(request, cancellationToken)
        );
    }
}