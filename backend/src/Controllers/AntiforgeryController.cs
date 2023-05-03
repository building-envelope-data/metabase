using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Server.AspNetCore;

namespace Metabase.Controllers
{
    public sealed class AntiforgeryController : Controller
    {
        private const string CookieKey = "XSRF-TOKEN";

        private readonly IAntiforgery _antiforgeryService;

        public AntiforgeryController(IAntiforgery antiforgeryService)
        {
            _antiforgeryService = antiforgeryService;
        }

        [HttpGet("~/antiforgery/token")]
        public IActionResult Token()
        {
            var tokens = _antiforgeryService.GetAndStoreTokens(HttpContext);
            if (tokens.RequestToken is null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            HttpContext.Response.Cookies.Append(
                CookieKey,
                tokens.RequestToken,
                new CookieOptions {
                    HttpOnly = false
                }
            );
            return new StatusCodeResult(StatusCodes.Status200OK);
        }
    }
}