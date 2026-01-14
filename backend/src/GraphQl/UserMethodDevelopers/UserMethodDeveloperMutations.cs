using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.Extensions;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.UserMethodDevelopers;

[ExtendObjectType(nameof(Mutation))]
public sealed class UserMethodDeveloperMutations
{
    [UseUserManager]
    [Authorize(Policy = AuthorizationPolicies.WritePolicy)]
    public async Task<AddUserMethodDeveloperPayload> AddUserMethodDeveloperAsync(
        AddUserMethodDeveloperInput input,
        ClaimsPrincipal claimsPrincipal,
        UserMethodDeveloperAuthorization authorization,
        ApplicationDbContext context,
        CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToAdd(
                claimsPrincipal,
                input.MethodId,
                cancellationToken
            )
           )
        {
            return new AddUserMethodDeveloperPayload(
                new AddUserMethodDeveloperError(
                    AddUserMethodDeveloperErrorCode.UNAUTHORIZED,
                    $"You are not authorized to add method developer relation for method ${input.MethodId}.",
                    []
                )
            );
        }

        var errors = new List<AddUserMethodDeveloperError>();
        if (!await context.Methods.AsQueryable()
                .Where(u => u.Id == input.MethodId)
                .AnyAsync(cancellationToken)
           )
        {
            errors.Add(
                new AddUserMethodDeveloperError(
                    AddUserMethodDeveloperErrorCode.UNKNOWN_METHOD,
                    "Unknown method.",
                    [nameof(input), nameof(input.MethodId).FirstCharToLower()]
                )
            );
        }

        if (!await context.Users.AsQueryable()
                .Where(i => i.Id == input.UserId)
                .AnyAsync(cancellationToken)
           )
        {
            errors.Add(
                new AddUserMethodDeveloperError(
                    AddUserMethodDeveloperErrorCode.UNKNOWN_USER,
                    "Unknown user.",
                    [nameof(input), nameof(input.UserId).FirstCharToLower()]
                )
            );
        }

        if (errors.Count is not 0)
        {
            return new AddUserMethodDeveloperPayload(errors.AsReadOnly());
        }

        if (await context.UserMethodDevelopers.AsQueryable()
                .Where(m =>
                    m.MethodId == input.MethodId
                    && m.UserId == input.UserId
                )
                .AnyAsync(cancellationToken)
           )
        {
            return new AddUserMethodDeveloperPayload(
                new AddUserMethodDeveloperError(
                    AddUserMethodDeveloperErrorCode.DUPLICATE,
                    "User method developer already exists.",
                    [nameof(input)]
                )
            );
        }

        var userMethodDeveloper = new UserMethodDeveloper
        {
            MethodId = input.MethodId,
            UserId = input.UserId,
            Pending = !await authorization.IsAuthorizedToConfirm(claimsPrincipal, input.UserId, cancellationToken)
        };
        context.UserMethodDevelopers.Add(userMethodDeveloper);
        await context.SaveChangesAsync(cancellationToken);
        return new AddUserMethodDeveloperPayload(userMethodDeveloper);
    }

    [UseUserManager]
    [Authorize(Policy = AuthorizationPolicies.WritePolicy)]
    public async Task<ConfirmUserMethodDeveloperPayload> ConfirmUserMethodDeveloperAsync(
        ConfirmUserMethodDeveloperInput input,
        ClaimsPrincipal claimsPrincipal,
        UserMethodDeveloperAuthorization authorization,
        ApplicationDbContext context,
        CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToConfirm(
                claimsPrincipal,
                input.UserId,
                cancellationToken
            )
           )
        {
            return new ConfirmUserMethodDeveloperPayload(
                new ConfirmUserMethodDeveloperError(
                    ConfirmUserMethodDeveloperErrorCode.UNAUTHORIZED,
                    $"You are not authorized to confirm method developer relation for user ${input.UserId}.",
                    []
                )
            );
        }

        var errors = new List<ConfirmUserMethodDeveloperError>();
        if (!await context.Methods.AsQueryable()
                .Where(u => u.Id == input.MethodId)
                .AnyAsync(cancellationToken)
           )
        {
            errors.Add(
                new ConfirmUserMethodDeveloperError(
                    ConfirmUserMethodDeveloperErrorCode.UNKNOWN_METHOD,
                    "Unknown method.",
                    [nameof(input), nameof(input.MethodId).FirstCharToLower()]
                )
            );
        }

        if (!await context.Users.AsQueryable()
                .Where(i => i.Id == input.UserId)
                .AnyAsync(cancellationToken)
           )
        {
            errors.Add(
                new ConfirmUserMethodDeveloperError(
                    ConfirmUserMethodDeveloperErrorCode.UNKNOWN_USER,
                    "Unknown user.",
                    [nameof(input), nameof(input.UserId).FirstCharToLower()]
                )
            );
        }

        if (errors.Count is not 0)
        {
            return new ConfirmUserMethodDeveloperPayload(errors.AsReadOnly());
        }

        var userMethodDeveloper =
            await context.UserMethodDevelopers.AsQueryable()
                .Where(r =>
                    r.MethodId == input.MethodId
                    && r.UserId == input.UserId
                )
                .SingleOrDefaultAsync(cancellationToken);
        if (userMethodDeveloper is null)
        {
            return new ConfirmUserMethodDeveloperPayload(
                new ConfirmUserMethodDeveloperError(
                    ConfirmUserMethodDeveloperErrorCode.UNKNOWN_DEVELOPER,
                    "Unknown developer.",
                    [nameof(input)]
                )
            );
        }

        userMethodDeveloper.Pending = false;
        await context.SaveChangesAsync(cancellationToken);
        return new ConfirmUserMethodDeveloperPayload(userMethodDeveloper);
    }

    [UseUserManager]
    [Authorize(Policy = AuthorizationPolicies.WritePolicy)]
    public async Task<RemoveUserMethodDeveloperPayload> RemoveUserMethodDeveloperAsync(
        RemoveUserMethodDeveloperInput input,
        ClaimsPrincipal claimsPrincipal,
        UserMethodDeveloperAuthorization authorization,
        ApplicationDbContext context,
        CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToRemove(
                claimsPrincipal,
                input.MethodId,
                cancellationToken
            )
           )
        {
            return new RemoveUserMethodDeveloperPayload(
                new RemoveUserMethodDeveloperError(
                    RemoveUserMethodDeveloperErrorCode.UNAUTHORIZED,
                    $"You are not authorized to remove method developer relation for method ${input.MethodId}.",
                    []
                )
            );
        }

        var errors = new List<RemoveUserMethodDeveloperError>();
        if (!await context.Methods.AsQueryable()
                .Where(u => u.Id == input.MethodId)
                .AnyAsync(cancellationToken)
           )
        {
            errors.Add(
                new RemoveUserMethodDeveloperError(
                    RemoveUserMethodDeveloperErrorCode.UNKNOWN_METHOD,
                    "Unknown method.",
                    [nameof(input), nameof(input.MethodId).FirstCharToLower()]
                )
            );
        }

        if (!await context.Users.AsQueryable()
                .Where(i => i.Id == input.UserId)
                .AnyAsync(cancellationToken)
           )
        {
            errors.Add(
                new RemoveUserMethodDeveloperError(
                    RemoveUserMethodDeveloperErrorCode.UNKNOWN_USER,
                    "Unknown user.",
                    [nameof(input), nameof(input.UserId).FirstCharToLower()]
                )
            );
        }

        if (errors.Count is not 0)
        {
            return new RemoveUserMethodDeveloperPayload(errors.AsReadOnly());
        }

        var userMethodDeveloper =
            await context.UserMethodDevelopers.AsQueryable()
                .Where(r =>
                    r.MethodId == input.MethodId
                    && r.UserId == input.UserId
                )
                .SingleOrDefaultAsync(cancellationToken);
        if (userMethodDeveloper is null)
        {
            return new RemoveUserMethodDeveloperPayload(
                new RemoveUserMethodDeveloperError(
                    RemoveUserMethodDeveloperErrorCode.UNKNOWN_DEVELOPER,
                    "Unknown developer.",
                    [nameof(input)]
                )
            );
        }

        context.UserMethodDevelopers.Remove(userMethodDeveloper);
        await context.SaveChangesAsync(cancellationToken);
        return new RemoveUserMethodDeveloperPayload(userMethodDeveloper);
    }
}