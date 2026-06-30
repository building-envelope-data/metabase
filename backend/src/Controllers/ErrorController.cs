using Metabase.ViewModels.Common;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace Metabase.Controllers;

public class ErrorController : Controller
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [Route("~/error")]
    public IActionResult Error()
    {
        var response = HttpContext.GetOpenIddictServerResponse() ?? HttpContext.GetOpenIddictClientResponse();
        if (response is not null)
        {
            return View(new ErrorViewModel
            {
                Error = response.Error,
                ErrorDescription = response.ErrorDescription
            });
        }
        return View(new ErrorViewModel());
    }
}