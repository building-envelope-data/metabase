using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenIddict.Client;

namespace Metabase.Authorization;

public sealed class BearerTokenSchemeHandler(
    IOptionsMonitor<BearerTokenSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    OpenIddictClientService openIddictClientService
    )
: AuthenticationHandler<BearerTokenSchemeOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        return HttpContextAuthentication.AuthenticateAsync(
            Context,
            openIddictClientService,
            Context.RequestAborted
        );
    }
}