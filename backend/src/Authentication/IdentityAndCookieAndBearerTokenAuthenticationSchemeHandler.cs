using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Metabase.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenIddict.Client;

namespace Metabase.Authentication;

public sealed class IdentityAndCookieAndBearerTokenAuthenticationSchemeHandler(
    IOptionsMonitor<IdentityAndCookieAndBearerTokenAuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    AuthenticationHandler authenticationHandler
)
: AuthenticationHandler<IdentityAndCookieAndBearerTokenAuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        return authenticationHandler.AuthenticateAsync(
            Context,
            Context.RequestAborted
        );
    }
}