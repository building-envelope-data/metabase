using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Core;
using Metabase.Data;
using Metabase.Data.OpenIdConnect;
using Metabase.Enumerations;

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
        var verifiedManufacturerIds =
            await Context.Institutions.AsNoTracking()
                .Where(i => i.State == InstitutionState.VERIFIED)
                .Where(i => i.ManufacturedComponentEdges.Any(c => c.ComponentId == componentId && !c.Pending))
                .Select(i => i.Id)
                .ToListAsync(cancellationToken);
        foreach (var verifiedManufacturerId in verifiedManufacturerIds)
        {
            if (await IsAtLeastAssistantOfInstitution(
                    user,
                    verifiedManufacturerId,
                    cancellationToken
                )
            )
            {
                return true;
            }
        }
        return false;
    }

    protected Task<bool> BelongsToVerifiedManufacturerOfComponent(
        OpenIdConnectApplication application,
        Guid componentId,
        CancellationToken cancellationToken
    )
    {
        return Context.Institutions.AsNoTracking()
            .Where(i => i.State == InstitutionState.VERIFIED)
            .Where(i => i.ManufacturedComponentEdges.Any(e => e.ComponentId == componentId && !e.Pending))
            .Where(manufacturer =>
                manufacturer.OpenIdConnectApplicationEdges.Any(e => e.ApplicationId == application.Id)
                || manufacturer.Manager != null && manufacturer.Manager.OpenIdConnectApplicationEdges.Any(e => e.ApplicationId == application.Id)
                || manufacturer.Manager != null && manufacturer.Manager.Manager != null && manufacturer.Manager.Manager.OpenIdConnectApplicationEdges.Any(e => e.ApplicationId == application.Id)
            )
            .AnyAsync(cancellationToken);
    }
}