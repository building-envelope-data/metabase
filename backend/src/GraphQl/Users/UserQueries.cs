using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.AspNetCore.Identity;

namespace Metabase.GraphQl.Users
{
    [ExtendObjectType(Name = nameof(GraphQl.Query))]
    public sealed class UserQueries
    {
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        public async Task<Data.User?> GetCurrentUserAsync(
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
            [ScopedService] UserManager<Data.User> userManager
            )
        {
            return await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
        }

        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UsePaging]
        /* TODO [UseProjection] // fails without an explicit error message in the logs */
        /* TODO [UseFiltering(typeof(UserFilterType))] // wait for https://github.com/ChilliCream/hotchocolate/issues/2672 and https://github.com/ChilliCream/hotchocolate/issues/2666 */
        [UseSorting]
        public IQueryable<Data.User> GetUsers(
            [ScopedService] Data.ApplicationDbContext context
            )
        {
            return context.Users;
        }

        // Inspired by https://github.com/dotnet/Scaffolding/blob/master/src/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Manage/Account.Manage.PersonalData.cs.cshtml
        // and https://github.com/dotnet/Scaffolding/blob/master/src/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Manage/Account.Manage.DownloadPersonalData.cs.cshtml
        [Authorize]
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        public async Task<PersonalUserDataPayload> GetPersonalUserDataAsync(
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
            [ScopedService] UserManager<Data.User> userManager
            )
        {
            var user = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
            if (user is null)
            {
                return new PersonalUserDataPayload(
                    new PersonalUserDataError(
                      PersonalUserDataErrorCode.UNKNOWN_USER,
                      $"Unable to load user with identifier {userManager.GetUserId(claimsPrincipal)}.",
                      Array.Empty<string>()
                      )
                    );
            }
            return new PersonalUserDataPayload(user);
        }

        // TODO https://github.com/dotnet/Scaffolding/blob/master/src/VS.Web.CG.Mvc/Templates/Identity/Bootstrap4/Pages/Account/Manage/Account.Manage.TwoFactorAuthentication.cs.cshtml
    }
}