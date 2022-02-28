using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using Metabase.Authorization;
using Metabase.Extensions;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.InstitutionMethodDevelopers
{
    [ExtendObjectType(nameof(Mutation))]
    public sealed class InstitutionMethodDeveloperMutations
    {
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        [Authorize(Policy = Configuration.AuthConfiguration.WritePolicy)]
        public async Task<ConfirmInstitutionMethodDeveloperPayload> ConfirmInstitutionMethodDeveloperAsync(
            ConfirmInstitutionMethodDeveloperInput input,
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
            [ScopedService] UserManager<Data.User> userManager,
            [ScopedService] Data.ApplicationDbContext context,
            CancellationToken cancellationToken
            )
        {
            if (!await InstitutionMethodDeveloperAuthorization.IsAuthorizedToConfirm(
                 claimsPrincipal,
                 input.InstitutionId,
                 userManager,
                 context,
                 cancellationToken
                 ).ConfigureAwait(false)
               )
            {
                return new ConfirmInstitutionMethodDeveloperPayload(
                    new ConfirmInstitutionMethodDeveloperError(
                      ConfirmInstitutionMethodDeveloperErrorCode.UNAUTHORIZED,
                      $"You are not authorized to confirm method developer relation for institution ${input.InstitutionId}.",
                      Array.Empty<string>()
                      )
                      );
            }
            var errors = new List<ConfirmInstitutionMethodDeveloperError>();
            if (!await context.Methods.AsQueryable()
                .Where(u => u.Id == input.MethodId)
                .AnyAsync(cancellationToken)
                .ConfigureAwait(false)
                )
            {
                errors.Add(
                    new ConfirmInstitutionMethodDeveloperError(
                      ConfirmInstitutionMethodDeveloperErrorCode.UNKNOWN_METHOD,
                      "Unknown method.",
                      new[] { nameof(input), nameof(input.MethodId).FirstCharToLower() }
                      )
                );
            }
            if (!await context.Institutions.AsQueryable()
                .Where(i => i.Id == input.InstitutionId)
                .AnyAsync(cancellationToken)
                .ConfigureAwait(false)
            )
            {
                errors.Add(
                    new ConfirmInstitutionMethodDeveloperError(
                      ConfirmInstitutionMethodDeveloperErrorCode.UNKNOWN_INSTITUTION,
                      "Unknown institution.",
                      new[] { nameof(input), nameof(input.InstitutionId).FirstCharToLower() }
                      )
                );
            }
            if (errors.Count is not 0)
            {
                return new ConfirmInstitutionMethodDeveloperPayload(errors.AsReadOnly());
            }
            var institutionMethodDeveloper =
                await context.InstitutionMethodDevelopers.AsQueryable()
                .Where(r =>
                       r.MethodId == input.MethodId
                    && r.InstitutionId == input.InstitutionId
                    )
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
            if (institutionMethodDeveloper is null)
            {
                return new ConfirmInstitutionMethodDeveloperPayload(
                    new ConfirmInstitutionMethodDeveloperError(
                      ConfirmInstitutionMethodDeveloperErrorCode.UNKNOWN_DEVELOPER,
                      "Unknown developer.",
                      new[] { nameof(input) }
                      )
                );
            }
            institutionMethodDeveloper.Pending = false;
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return new ConfirmInstitutionMethodDeveloperPayload(institutionMethodDeveloper);
        }
    }
}