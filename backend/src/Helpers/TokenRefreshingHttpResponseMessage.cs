using System.Net.Http;
using static OpenIddict.Client.OpenIddictClientModels;

namespace Metabase.Helpers;

internal sealed class TokenRefreshingHttpResponseMessage
: HttpResponseMessage
{
    public RefreshTokenAuthenticationResult RefreshTokenAuthenticationResult { get; }

    public TokenRefreshingHttpResponseMessage(
        RefreshTokenAuthenticationResult result,
        HttpResponseMessage response
    )
    {
        RefreshTokenAuthenticationResult = result;
        Content = response.Content;
        StatusCode = response.StatusCode;
        Version = response.Version;
        foreach (var header in response.Headers)
        {
            Headers.Add(header.Key, header.Value);
        }
    }
}