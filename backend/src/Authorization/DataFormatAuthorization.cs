using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Metabase.Data;

namespace Metabase.Authorization;

public sealed class DataFormatAuthorization(
    ApplicationDbContext context,
    UserManager<User> userManager
) : CommonAuthorization(context, userManager)
{
    internal async Task<bool> IsAuthorizedToCreateDataFormatForInstitution(
        ClaimsPrincipal claimsPrincipal,
        Guid institutionId,
        CancellationToken cancellationToken
    )
    {
        var user = await GetUserAsync(claimsPrincipal);
        return user is not null
               && await IsAtLeastAssistantOfVerifiedInstitution(
                   user,
                   institutionId,
                   cancellationToken
               );
    }

    internal async Task<bool> IsAuthorizedToUpdate(
        ClaimsPrincipal claimsPrincipal,
        Guid dataFormatId,
        CancellationToken cancellationToken
    )
    {
        var user = await GetUserAsync(claimsPrincipal);
        return user is not null &&
               await IsAtLeastAssistantOfVerifiedDataFormatManager(
                   user,
                   dataFormatId,
                   cancellationToken
               );
    }

    private async Task<bool> IsAtLeastAssistantOfVerifiedDataFormatManager(
        User user,
        Guid dataFormatId,
        CancellationToken cancellationToken
    )
    {
        var wrappedManagerId =
            await Context.DataFormats.AsNoTracking()
                .Where(x => x.Id == dataFormatId)
                .Select(x => new { x.ManagerId })
                .SingleOrDefaultAsync(cancellationToken);
        if (wrappedManagerId is null)
        {
            return false;
        }

        return await IsAtLeastAssistantOfVerifiedInstitution(user, wrappedManagerId.ManagerId, cancellationToken);
    }
}