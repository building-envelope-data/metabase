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
using Metabase.Enumerations;

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
            application => BelongsToVerifiedInstitution(
                application,
                institutionId,
                cancellationToken
            ),
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
            application => BelongsToVerifiedDatabaseOperator(
                application,
                databaseId,
                cancellationToken
            ),
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
            application => BelongsToVerifiedDatabaseOperator(
                application,
                databaseId,
                cancellationToken
            ),
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
                .Where(d => d.Id == databaseId)
                .Select(d => new { d.OperatorId })
                .SingleOrDefaultAsync(cancellationToken)
        )?.OperatorId;
    }

    private Task<bool> BelongsToVerifiedDatabaseOperator(
        OpenIdConnectApplication application,
        Guid databaseId,
        CancellationToken cancellationToken
    )
    {
        return Context.Databases.AsNoTracking()
            .Where(d => d.Id == databaseId)
            .Where(d => d.Operator != null && d.Operator.State == InstitutionState.VERIFIED)
            .Where(d => d.Operator != null && d.Operator.OpenIdConnectApplicationEdges.Any(e => e.ApplicationId == application.Id))
            .AnyAsync(cancellationToken);
    }
}