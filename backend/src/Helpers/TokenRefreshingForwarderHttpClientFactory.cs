using System.Net.Http;
using OpenIddict.Client;
using Yarp.ReverseProxy.Forwarder;

namespace Metabase.Helpers;

internal sealed class TokenRefreshingForwarderHttpClientFactory(
    OpenIddictClientService service
)
: ForwarderHttpClientFactory
{
    protected override HttpMessageHandler WrapHandler(
        ForwarderHttpClientContext context,
        HttpMessageHandler handler
    )
    {
        return new TokenRefreshingDelegatingHandler(service, handler);
    }
}
