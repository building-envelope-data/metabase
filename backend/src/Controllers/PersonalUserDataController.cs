using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Metabase.Controllers
{
    // Inspired by https://github.com/dotnet/Scaffolding/blob/main/src/Scaffolding/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Manage/Account.Manage.DownloadPersonalData.cs.cshtml
    public sealed class PersonalUserDataController : Controller
    {
        private readonly UserManager<Data.User> _userManager;

        public PersonalUserDataController(
            UserManager<Data.User> userManager
            )
        {
            _userManager = userManager;
        }

        [HttpGet("~/personal-user-data")]
        public async Task<IActionResult> GetAsync()
        {
            var user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
            if (user is null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            var personalData = new Dictionary<string, string>();
            var personalDataProps = typeof(Data.User).GetProperties().Where(
                            prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));
            foreach (var prop in personalDataProps)
            {
                personalData.Add(prop.Name, prop.GetValue(user)?.ToString() ?? "null");
            }
            var logins = await _userManager.GetLoginsAsync(user).ConfigureAwait(false);
            foreach (var login in logins)
            {
                personalData.Add($"{login.LoginProvider} external login provider key", login.ProviderKey);
            }
            Response.Headers.Add("Content-Disposition", "attachment; filename=PersonalUserData.json");
            return new FileContentResult(JsonSerializer.SerializeToUtf8Bytes(personalData), "application/json");
        }
    }
}