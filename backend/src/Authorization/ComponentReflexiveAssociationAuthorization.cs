using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Metabase.Authorization
{
    public static class ComponentReflexiveAssociationAuthorization
    {
        internal static async Task<bool> IsAtLeastAssistantOfOneVerifiedManufacturerOfComponent(
            Data.User user,
            Guid componentId,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
        )
        {
            var manufacturerIds =
                await context.Institutions.AsQueryable()
                .Where(i => i.ManufacturedComponents.Any(c => c.Id == componentId))
                .Select(i => i.Id)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
            foreach (var manufacturerId in manufacturerIds)
            {
                if (await CommonAuthorization.IsAtLeastAssistantOfVerifiedInstitution(
                        user,
                        manufacturerId,
                        context,
                        cancellationToken
                    ).ConfigureAwait(false)
                    &&
                    await CommonAuthorization.IsVerifiedManufacturerOfComponent(
                        manufacturerId,
                        componentId,
                        context,
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
}