using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Core;
using Metabase.Data;
using Metabase.Data.OpenIdConnect;

namespace Metabase.Authorization;

public sealed class DatabaseAuthorization(
    ApplicationDbContext context,
    UserManager<User> userManager,
    OpenIddictApplicationManager<OpenIdConnectApplication> applicationManager
) : CommonAuthorization(context, userManager, applicationManager)
{
    internal Task<bool> IsAuthorizedToCreateDatabaseForInstitution(
        ClaimsPrincipal claimsPrincipal,
        Guid institutionId,
        CancellationToken cancellationToken
    )
    {
        return AuthorizeAsync(
            claimsPrincipal,
            user => IsAtLeastAssistantOfVerifiedInstitution(
                user,
                institutionId,
                cancellationToken
            ),
            application => Task.FromResult(false),
            cancellationToken
        );
    }

    internal Task<bool> IsAuthorizedToUpdate(
        ClaimsPrincipal claimsPrincipal,
        Guid databaseId,
        CancellationToken cancellationToken
    )
    {
        return AuthorizeAsync(
            claimsPrincipal,
            user => IsAtLeastAssistantOfVerifiedDatabaseOperator(
                user,
                databaseId,
                cancellationToken
            ),
            application => Task.FromResult(false),
            cancellationToken
        );
    }

    internal Task<bool> IsAuthorizedToVerify(
        ClaimsPrincipal claimsPrincipal,
        Guid databaseId,
        CancellationToken cancellationToken
    )
    {
        return AuthorizeAsync(
            claimsPrincipal,
            user => IsAtLeastAssistantOfVerifiedDatabaseOperator(
                user,
                databaseId,
                cancellationToken
            ),
            application => Task.FromResult(false),
            cancellationToken
        );
    }

    private async Task<bool> IsAtLeastAssistantOfVerifiedDatabaseOperator(
        User user,
        Guid databaseId,
        CancellationToken cancellationToken
    )
    {
        var operatorId = await QueryOperatorId(databaseId, cancellationToken);
        return operatorId is not null
            && await IsAtLeastAssistantOfVerifiedInstitution(user, operatorId ?? Guid.Empty, cancellationToken);
    }

    private async Task<Guid?> QueryOperatorId(Guid databaseId, CancellationToken cancellationToken)
    {
        return (
            await Context.Databases.AsNoTracking()
                .Where(x => x.Id == databaseId)
                .Select(x => new { x.OperatorId })
                .SingleOrDefaultAsync(cancellationToken)
        )?.OperatorId;
    }
}