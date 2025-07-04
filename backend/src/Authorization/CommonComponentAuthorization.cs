using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Metabase.Data;

namespace Metabase.Authorization;

public abstract class CommonComponentAuthorization(
    ApplicationDbContext context,
    UserManager<User> userManager
) : CommonAuthorization(context, userManager)
{
    protected async Task<bool> IsAtLeastAssistantOfOneVerifiedManufacturerOfComponent(
        User user,
        Guid componentId,
        CancellationToken cancellationToken
    )
    {
        var manufacturerIds =
            await Context.Institutions.AsNoTracking()
                .Where(i => i.ManufacturedComponents.Any(c => c.Id == componentId))
                .Select(i => i.Id)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        foreach (var manufacturerId in manufacturerIds)
        {
            if (await IsAtLeastAssistantOfVerifiedInstitution(
                    user,
                    manufacturerId,
                    cancellationToken
                ).ConfigureAwait(false)
                &&
                await IsVerifiedManufacturerOfComponent(
                    manufacturerId,
                    componentId,
                    cancellationToken
                ).ConfigureAwait(false)
               )
            {
                return true;
            }
        }

        return false;
    }
}