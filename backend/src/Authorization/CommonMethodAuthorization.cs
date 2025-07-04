using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Metabase.Data;

namespace Metabase.Authorization;

public abstract class CommonMethodAuthorization(
    ApplicationDbContext context,
    UserManager<User> userManager
) : CommonAuthorization(context, userManager)
{
    protected async Task<bool> IsAtLeastAssistantOfVerifiedMethodManager(
        User user,
        Guid methodId,
        CancellationToken cancellationToken
    )
    {
        var wrappedManagerId =
            await Context.Methods.AsNoTracking()
                .Where(x => x.Id == methodId)
                .Select(x => new { x.ManagerId })
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
        if (wrappedManagerId is null)
        {
            return false;
        }

        return await IsAtLeastAssistantOfVerifiedInstitution(
            user, wrappedManagerId.ManagerId, cancellationToken
        );
    }
}