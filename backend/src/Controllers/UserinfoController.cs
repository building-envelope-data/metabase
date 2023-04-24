using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Metabase.Controllers
{
    // Inspired by https://github.com/openiddict/openiddict-samples/blob/dev/samples/Velusia/Velusia.Server/Controllers/UserinfoController.cs
    public sealed class UserinfoController : Controller
    {
        private readonly UserManager<Data.User> _userManager;

        public UserinfoController(UserManager<Data.User> userManager)
            => _userManager = userManager;

        // GET: /connect/userinfo
        [Authorize(AuthenticationSchemes = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)]
        [HttpGet("~/connect/userinfo"), HttpPost("~/connect/userinfo"), Produces("application/json")]
        public async Task<IActionResult> Userinfo()
        {
            var user = await _userManager.FindByIdAsync(
                User.GetClaim(Claims.Subject)
                ?? throw new InvalidOperationException("The subject claim of the claims principal is `null`.")
                ).ConfigureAwait(false);
            if (user is null)
            {
                return Challenge(
                    authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties(new Dictionary<string, string?>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidToken,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                            "The specified access token is bound to an account that no longer exists."
                    }));
            }

            var claims = new Dictionary<string, object>(StringComparer.Ordinal)
            {
                // Note: the "sub" claim is a mandatory claim and must be included in the JSON response.
                [Claims.Subject] = await _userManager.GetUserIdAsync(user).ConfigureAwait(false)
            };

            if (User.HasScope(Scopes.Email))
            {
                var email = await _userManager.GetEmailAsync(user).ConfigureAwait(false);
                if (email is not null) claims[Claims.Email] = email;
                claims[Claims.EmailVerified] = await _userManager.IsEmailConfirmedAsync(user).ConfigureAwait(false);
            }

            if (User.HasScope(Scopes.Phone))
            {
                var phoneNumber = await _userManager.GetPhoneNumberAsync(user).ConfigureAwait(false);
                if (phoneNumber is not null) claims[Claims.PhoneNumber] = phoneNumber;
                claims[Claims.PhoneNumberVerified] = await _userManager.IsPhoneNumberConfirmedAsync(user).ConfigureAwait(false);
            }

            if (User.HasScope(Scopes.Roles))
            {
                claims[Claims.Role] = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            }

            // Note: the complete list of standard claims supported by the OpenID Connect specification
            // can be found here: http://openid.net/specs/openid-connect-core-1_0.html#StandardClaims

            return Ok(claims);
        }
    }
}