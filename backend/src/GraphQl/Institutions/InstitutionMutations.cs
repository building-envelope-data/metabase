using System;
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

namespace Metabase.GraphQl.Institutions
{
    [ExtendObjectType(nameof(Mutation))]
    public sealed class InstitutionMutations
    {
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        [Authorize(Policy = Configuration.AuthConfiguration.WritePolicy)]
        public async Task<CreateInstitutionPayload> CreateInstitutionAsync(
            CreateInstitutionInput input,
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
            [ScopedService] UserManager<Data.User> userManager,
            [ScopedService] Data.ApplicationDbContext context,
            CancellationToken cancellationToken
            )
        {
            if (input.ManagerId is not null && !await InstitutionAuthorization.IsAuthorizedToCreateInstitutionManagedByInstitution(
                 claimsPrincipal,
                 input.ManagerId ?? Guid.Empty,
                 userManager,
                 context,
                 cancellationToken
                 ).ConfigureAwait(false)
            )
            {
                return new CreateInstitutionPayload(
                    new CreateInstitutionError(
                      CreateInstitutionErrorCode.UNAUTHORIZED,
                      "You are not authorized to create components for the institution.",
                      new[] { nameof(input), nameof(input.ManagerId).FirstCharToLower() }
                    )
                );
            }
            var user = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
            if (user is null)
            {
                return new CreateInstitutionPayload(
                    new CreateInstitutionError(
                      CreateInstitutionErrorCode.UNKNOWN,
                      "Unknown user.",
                      Array.Empty<string>()
                      )
                );
            }
            if (input.OwnerIds.Count is 0 && input.ManagerId is null)
            {
                return new CreateInstitutionPayload(
                    new CreateInstitutionError(
                      CreateInstitutionErrorCode.NEITHER_OWNER_NOR_MANAGER,
                      "Neither owner nor manager given.",
                      new[] { nameof(input) }
                      )
                );
            }
            var unknownOwnerIds =
                input.OwnerIds.Except(
                    await context.Users.AsQueryable()
                    .Where(u => input.OwnerIds.Contains(u.Id))
                    .Select(u => u.Id)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false)
                );
            if (unknownOwnerIds.Any())
            {
                return new CreateInstitutionPayload(
                    new CreateInstitutionError(
                      CreateInstitutionErrorCode.UNKNOWN_OWNERS,
                      $"There are no users with identifier(s) {string.Join(", ", unknownOwnerIds)}.",
                      new[] { nameof(input), nameof(input.OwnerIds).FirstCharToLower() }
                      )
                );
            }
            if (input.ManagerId is not null &&
                !await context.Institutions.AsQueryable()
                .AnyAsync(
                    x => x.Id == input.ManagerId,
                    cancellationToken: cancellationToken
                 )
                .ConfigureAwait(false)
            )
            {
                return new CreateInstitutionPayload(
                    new CreateInstitutionError(
                        CreateInstitutionErrorCode.UNKNOWN_MANAGER,
                        "Unknown manager.",
                      new[] { nameof(input), nameof(input.ManagerId).FirstCharToLower() }
                    )
                );
            }
            var institution = new Data.Institution(
                name: input.Name,
                abbreviation: input.Abbreviation,
                description: input.Description,
                websiteLocator: input.WebsiteLocator,
                publicKey: input.PublicKey,
                state: input.State
            )
            {
                ManagerId = input.ManagerId
            };
            foreach (var ownerId in input.OwnerIds)
            {
                institution.RepresentativeEdges.Add(
                    new Data.InstitutionRepresentative
                    {
                        UserId = ownerId,
                        Role = Enumerations.InstitutionRepresentativeRole.OWNER,
                        Pending = ownerId != user.Id
                    }
                );
            }
            context.Institutions.Add(institution);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return new CreateInstitutionPayload(institution);
        }

        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        [Authorize(Policy = Configuration.AuthConfiguration.WritePolicy)]
        public async Task<UpdateInstitutionPayload> UpdateInstitutionAsync(
            UpdateInstitutionInput input,
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
            [ScopedService] UserManager<Data.User> userManager,
            [ScopedService] Data.ApplicationDbContext context,
            CancellationToken cancellationToken
            )
        {
            if (!await InstitutionAuthorization.IsAuthorizedToUpdateInstitution(
                 claimsPrincipal,
                 input.InstitutionId,
                 userManager,
                 context,
                 cancellationToken
                 ).ConfigureAwait(false)
               )
            {
                return new UpdateInstitutionPayload(
                    new UpdateInstitutionError(
                      UpdateInstitutionErrorCode.UNAUTHORIZED,
                      "You are not authorized to update the institution.",
                      Array.Empty<string>()
                      )
                      );
            }
            var institution =
                await context.Institutions.AsQueryable()
                .Where(i => i.Id == input.InstitutionId)
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
            if (institution is null)
            {
                return new UpdateInstitutionPayload(
                    new UpdateInstitutionError(
                      UpdateInstitutionErrorCode.UNKNOWN_INSTITUTION,
                      "Unknown institution.",
                      new[] { nameof(input), nameof(input.InstitutionId).FirstCharToLower() }
                      )
                      );
            }
            institution.Update(
                name: input.Name,
                abbreviation: input.Abbreviation,
                description: input.Description,
                websiteLocator: input.WebsiteLocator,
                publicKey: input.PublicKey,
                state: input.State
            );
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return new UpdateInstitutionPayload(institution);
        }

        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        [Authorize(Policy = Configuration.AuthConfiguration.WritePolicy)]
        public async Task<DeleteInstitutionPayload> DeleteInstitutionAsync(
            DeleteInstitutionInput input,
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
            [ScopedService] UserManager<Data.User> userManager,
            [ScopedService] Data.ApplicationDbContext context,
            CancellationToken cancellationToken
            )
        {
            if (!await InstitutionAuthorization.IsAuthorizedToDeleteInstitution(
                 claimsPrincipal,
                 input.InstitutionId,
                 userManager,
                 context,
                 cancellationToken
                 ).ConfigureAwait(false)
               )
            {
                return new DeleteInstitutionPayload(
                    new DeleteInstitutionError(
                      DeleteInstitutionErrorCode.UNAUTHORIZED,
                      "You are not authorized to delete the institution.",
                      Array.Empty<string>()
                      )
                      );
            }
            var institution =
                await context.Institutions.AsQueryable()
                .Where(i => i.Id == input.InstitutionId)
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
            if (institution is null)
            {
                return new DeleteInstitutionPayload(
                    new DeleteInstitutionError(
                      DeleteInstitutionErrorCode.UNKNOWN_INSTITUTION,
                      "Unknown institution.",
                      new[] { nameof(input), nameof(input.InstitutionId).FirstCharToLower() }
                      )
                      );
            }
            context.Institutions.Remove(institution);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return new DeleteInstitutionPayload();
        }
    }
}