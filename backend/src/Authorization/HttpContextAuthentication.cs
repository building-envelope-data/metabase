using System.Threading.Tasks;
using Metabase.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using OpenIddict.Validation.AspNetCore;

namespace Metabase.Authorization
{
    public static class HttpContextAuthentication
    {
        public static async Task<AuthenticateResult> Authenticate(
            HttpContext httpContext
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
            var identityAuthenticateResult = await httpContext.AuthenticateAsync(AuthConfiguration.IdentityConstantsApplicationScheme).ConfigureAwait(false);
            if (identityAuthenticateResult.Succeeded && identityAuthenticateResult.Principal is not null)
            {
                httpContext.User = identityAuthenticateResult.Principal;
                return identityAuthenticateResult;
            }
            // For the Next.js Web frontend, the metabase acts as OpenId Connect
            // Client and uses the cookie scheme for authentication. See
            // `AuthConfiguration#ConfigureAuthenticationAndAuthorizationServices`
            // for the configuration of the "cookie scheme" cookie. This cookie
            // is set by methods in `AuthenticationController` and is related to
            // `OpenIddictBuilder#AddClient` in
            // `AuthConfiguration#ConfigureOpenIddictServices`.
            var cookieAuthenticateResult = await httpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme).ConfigureAwait(false);
            if (cookieAuthenticateResult.Succeeded && cookieAuthenticateResult.Principal is not null)
            {
                httpContext.User = cookieAuthenticateResult.Principal;
                return cookieAuthenticateResult;
            }
            // For third-party frontends, the metabase acts as resource server
            // and uses authorization-header bearer tokens for authentication,
            // that is JavaScript Web Tokens (JWT), aka, Access Tokens, provided
            // as `Authorization` HTTP header with the prefix `Bearer` as issued
            // by OpenIddict. This Access Token includes Scopes and Claims. The
            // scheme is configured in
            // `AuthConfiguration#ConfigureOpenIddictServices` by
            // `OpenIddictBuilder#AddValidation`.
            var jwtAuthenticateResult = await httpContext.AuthenticateAsync(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme).ConfigureAwait(false);
            if (jwtAuthenticateResult.Succeeded && jwtAuthenticateResult.Principal is not null)
            {
                httpContext.User = jwtAuthenticateResult.Principal;
                return jwtAuthenticateResult;
            }
            return AuthenticateResult.Fail("All available authentication schemes failed or yielded no claims principal.");
        }
    }
}