using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using Metabase.Authorization;
using Metabase.Extensions;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.UserMethodDevelopers
{
    [ExtendObjectType(nameof(Mutation))]
    public sealed class UserMethodDeveloperMutations
    {
        [UseUserManager]
        [Authorize(Policy = Configuration.AuthConfiguration.WritePolicy)]
        public async Task<ConfirmUserMethodDeveloperPayload> ConfirmUserMethodDeveloperAsync(
            ConfirmUserMethodDeveloperInput input,
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
            [Service(ServiceKind.Resolver)] UserManager<Data.User> userManager,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
            )
        {
            if (!await UserMethodDeveloperAuthorization.IsAuthorizedToConfirm(
                 claimsPrincipal,
                 input.UserId,
                 userManager
                 ).ConfigureAwait(false)
               )
            {
                return new ConfirmUserMethodDeveloperPayload(
                    new ConfirmUserMethodDeveloperError(
                      ConfirmUserMethodDeveloperErrorCode.UNAUTHORIZED,
                      $"You are not authorized to confirm method developer relation for user ${input.UserId}.",
                      Array.Empty<string>()
                      )
                      );
            }
            var errors = new List<ConfirmUserMethodDeveloperError>();
            if (!await context.Methods.AsQueryable()
                .Where(u => u.Id == input.MethodId)
                .AnyAsync(cancellationToken)
                .ConfigureAwait(false)
                )
            {
                errors.Add(
                    new ConfirmUserMethodDeveloperError(
                      ConfirmUserMethodDeveloperErrorCode.UNKNOWN_METHOD,
                      "Unknown method.",
                      new[] { nameof(input), nameof(input.MethodId).FirstCharToLower() }
                      )
                );
            }
            if (!await context.Users.AsQueryable()
                .Where(i => i.Id == input.UserId)
                .AnyAsync(cancellationToken)
                .ConfigureAwait(false)
            )
            {
                errors.Add(
                    new ConfirmUserMethodDeveloperError(
                      ConfirmUserMethodDeveloperErrorCode.UNKNOWN_USER,
                      "Unknown user.",
                      new[] { nameof(input), nameof(input.UserId).FirstCharToLower() }
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
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
            if (userMethodDeveloper is null)
            {
                return new ConfirmUserMethodDeveloperPayload(
                    new ConfirmUserMethodDeveloperError(
                      ConfirmUserMethodDeveloperErrorCode.UNKNOWN_DEVELOPER,
                      "Unknown developer.",
                      new[] { nameof(input) }
                      )
                );
            }
            userMethodDeveloper.Pending = false;
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return new ConfirmUserMethodDeveloperPayload(userMethodDeveloper);
        }
    }
}