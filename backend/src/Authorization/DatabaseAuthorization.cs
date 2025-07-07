using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Metabase.Data;

namespace Metabase.Authorization;

public sealed class DatabaseAuthorization(
    ApplicationDbContext context,
    UserManager<User> userManager
) : CommonAuthorization(context, userManager)
{
    internal async Task<bool> IsAuthorizedToCreateDatabaseForInstitution(
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
        Guid databaseId,
        CancellationToken cancellationToken
    )
    {
        var user = await GetUserAsync(claimsPrincipal);
        return user is not null &&
               await IsAtLeastAssistantOfVerifiedDatabaseOperator(
                   user,
                   databaseId,
                   cancellationToken
               );
    }

    internal async Task<bool> IsAuthorizedToVerify(
        ClaimsPrincipal claimsPrincipal,
        Guid databaseId,
        CancellationToken cancellationToken
    )
    {
        var user = await GetUserAsync(claimsPrincipal);
        return user is not null &&
               await IsAtLeastAssistantOfVerifiedDatabaseOperator(
                   user,
                   databaseId,
                   cancellationToken
               );
    }

    private async Task<bool> IsAtLeastAssistantOfVerifiedDatabaseOperator(
        User user,
        Guid databaseId,
        CancellationToken cancellationToken
    )
    {
        var wrappedOperatorId =
            await Context.Databases.AsNoTracking()
                .Where(x => x.Id == databaseId)
                .Select(x => new { x.OperatorId })
                .SingleOrDefaultAsync(cancellationToken);
        if (wrappedOperatorId is null)
        {
            return false;
        }

        return await IsAtLeastAssistantOfVerifiedInstitution(user, wrappedOperatorId.OperatorId, cancellationToken);
    }
}