using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Metabase.Authorization;

public static class CommonMethodAuthorization
{
    internal static async Task<bool> IsAtLeastAssistantOfVerifiedMethodManager(
        Data.User user,
        Guid methodId,
        Data.ApplicationDbContext context,
        CancellationToken cancellationToken
    )
    {
        var wrappedManagerId =
            await context.Methods.AsQueryable()
                .Where(x => x.Id == methodId)
                .Select(x => new { x.ManagerId })
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
        if (wrappedManagerId is null) return false;

        return await CommonAuthorization.IsAtLeastAssistantOfVerifiedInstitution(
            user, wrappedManagerId.ManagerId, context, cancellationToken
        );
    }
}