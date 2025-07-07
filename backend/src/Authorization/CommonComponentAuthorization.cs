using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Core;
using Metabase.Data;
using Metabase.Data.OpenIdConnect;

namespace Metabase.Authorization;

public abstract class CommonComponentAuthorization(
    ApplicationDbContext context,
    UserManager<User> userManager,
    OpenIddictApplicationManager<OpenIdConnectApplication> applicationManager
) : CommonAuthorization(context, userManager, applicationManager)
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
                .ToListAsync(cancellationToken);
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