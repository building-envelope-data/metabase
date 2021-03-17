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

namespace Metabase.GraphQl.Institutions
{
    [ExtendObjectType(Name = nameof(Mutation))]
    public sealed class InstitutionMutations
    {
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [Authorize(Policy = Configuration.AuthConfiguration.WritePolicy)]
        public async Task<CreateInstitutionPayload> CreateInstitutionAsync(
            CreateInstitutionInput input,
            [ScopedService] Data.ApplicationDbContext context,
            CancellationToken cancellationToken
            )
        {
            if (input.OwnerIds.Count is 0)
            {
                return new CreateInstitutionPayload(
                    new CreateInstitutionError(
                      CreateInstitutionErrorCode.NO_OWNER,
                      "No owner assigned.",
                      new[] { nameof(input), nameof(input.OwnerIds).FirstCharToLower() }
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
            var institution = new Data.Institution(
                name: input.Name,
                abbreviation: input.Abbreviation,
                description: input.Description,
                websiteLocator: input.WebsiteLocator,
                publicKey: input.PublicKey,
                state: input.State
            );
            foreach (var ownerId in input.OwnerIds)
            {
                institution.RepresentativeEdges.Add(
                    new Data.InstitutionRepresentative
                    {
                        UserId = ownerId,
                        Role = Enumerations.InstitutionRepresentativeRole.OWNER
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

        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        [Authorize(Policy = Configuration.AuthConfiguration.WritePolicy)]
        public async Task<AddInstitutionRepresentativePayload> AddInstitutionRepresentativeAsync(
            AddInstitutionRepresentativeInput input,
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
            [ScopedService] UserManager<Data.User> userManager,
            [ScopedService] Data.ApplicationDbContext context,
            CancellationToken cancellationToken
            )
        {
            if (!await InstitutionAuthorization.IsAuthorizedToManageRepresentatives(
                 claimsPrincipal,
                 input.InstitutionId,
                 userManager,
                 context,
                 cancellationToken
                 ).ConfigureAwait(false)
               )
            {
                return new AddInstitutionRepresentativePayload(
                    new AddInstitutionRepresentativeError(
                      AddInstitutionRepresentativeErrorCode.UNAUTHORIZED,
                      "You are not authorized to add institution representatives.",
                      Array.Empty<string>()
                      )
                      );
            }
            var errors = new List<AddInstitutionRepresentativeError>();
            if (!await context.Institutions.AsQueryable()
                .Where(i => i.Id == input.InstitutionId)
                .AnyAsync(cancellationToken)
                .ConfigureAwait(false)
            )
            {
                errors.Add(
                    new AddInstitutionRepresentativeError(
                      AddInstitutionRepresentativeErrorCode.UNKNOWN_INSTITUTION,
                      "Unknown institution.",
                      new[] { nameof(input), nameof(input.InstitutionId).FirstCharToLower() }
                      )
                );
            }
            if (!await context.Users.AsQueryable()
                .Where(u => u.Id == input.UserId)
                .AnyAsync(cancellationToken)
                .ConfigureAwait(false)
                )
            {
                errors.Add(
                    new AddInstitutionRepresentativeError(
                      AddInstitutionRepresentativeErrorCode.UNKNOWN_USER,
                      "Unknown user.",
                      new[] { nameof(input), nameof(input.UserId).FirstCharToLower() }
                      )
                );
            }
            if (errors.Count is not 0)
            {
                return new AddInstitutionRepresentativePayload(errors.AsReadOnly());
            }
            if (await context.InstitutionRepresentatives.AsQueryable()
                .Where(r =>
                       r.InstitutionId == input.InstitutionId
                    && r.UserId == input.UserId
                    )
                .AnyAsync(cancellationToken)
                .ConfigureAwait(false)
                )
            {
                return new AddInstitutionRepresentativePayload(
                    new AddInstitutionRepresentativeError(
                      AddInstitutionRepresentativeErrorCode.DUPLICATE,
                      "Institution representative already exists.",
                      new[] { nameof(input) }
                      )
                );
            }
            var institutionRepresentative = new Data.InstitutionRepresentative
            {
                InstitutionId = input.InstitutionId,
                UserId = input.UserId,
                Role = input.Role
            };
            context.InstitutionRepresentatives.Add(institutionRepresentative);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return new AddInstitutionRepresentativePayload(institutionRepresentative);
        }

        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        [Authorize(Policy = Configuration.AuthConfiguration.WritePolicy)]
        public async Task<RemoveInstitutionRepresentativePayload> RemoveInstitutionRepresentativeAsync(
            RemoveInstitutionRepresentativeInput input,
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
            [ScopedService] UserManager<Data.User> userManager,
            [ScopedService] Data.ApplicationDbContext context,
            CancellationToken cancellationToken
            )
        {
            if (!await InstitutionAuthorization.IsAuthorizedToManageRepresentatives(
                 claimsPrincipal,
                 input.InstitutionId,
                 userManager,
                 context,
                 cancellationToken
                 ).ConfigureAwait(false)
               )
            {
                return new RemoveInstitutionRepresentativePayload(
                    new RemoveInstitutionRepresentativeError(
                      RemoveInstitutionRepresentativeErrorCode.UNAUTHORIZED,
                      "You are not authorized to remove institution representatives.",
                      Array.Empty<string>()
                      )
                      );
            }
            var errors = new List<RemoveInstitutionRepresentativeError>();
            if (!await context.Institutions.AsQueryable()
                .Where(i => i.Id == input.InstitutionId)
                .AnyAsync(cancellationToken)
                .ConfigureAwait(false)
                )
            {
                errors.Add(
                    new RemoveInstitutionRepresentativeError(
                      RemoveInstitutionRepresentativeErrorCode.UNKNOWN_INSTITUTION,
                      "Unknown institution.",
                      new[] { nameof(input), nameof(input.InstitutionId).FirstCharToLower() }
                      )
                );
            }
            if (!await context.Users.AsQueryable()
                .Where(u => u.Id == input.UserId)
                .AnyAsync(cancellationToken)
                .ConfigureAwait(false)
                )
            {
                errors.Add(
                    new RemoveInstitutionRepresentativeError(
                      RemoveInstitutionRepresentativeErrorCode.UNKNOWN_USER,
                      "Unknown user.",
                      new[] { nameof(input), nameof(input.UserId).FirstCharToLower() }
                      )
                );
            }
            if (errors.Count is not 0)
            {
                return new RemoveInstitutionRepresentativePayload(errors.AsReadOnly());
            }
            var institutionRepresentative =
                await context.InstitutionRepresentatives.AsQueryable()
                .Where(r =>
                       r.InstitutionId == input.InstitutionId
                    && r.UserId == input.UserId
                    )
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
            if (institutionRepresentative is null)
            {
                return new RemoveInstitutionRepresentativePayload(
                    new RemoveInstitutionRepresentativeError(
                      RemoveInstitutionRepresentativeErrorCode.UNKNOWN_REPRESENTATIVE,
                      "Unknown representative.",
                      new[] { nameof(input) }
                      )
                );
            }
            if (institutionRepresentative.Role == Enumerations.InstitutionRepresentativeRole.OWNER
                && !await ExistsOtherInstitutionOwner(
                        institutionRepresentative.InstitutionId,
                        institutionRepresentative.UserId,
                        context,
                        cancellationToken
                ).ConfigureAwait(false)
                )
            {
                return new RemoveInstitutionRepresentativePayload(
                    new RemoveInstitutionRepresentativeError(
                      RemoveInstitutionRepresentativeErrorCode.LAST_OWNER,
                      "Cannot remove last owner.",
                      new[] { nameof(input) }
                      )
                );
            }
            context.InstitutionRepresentatives.Remove(institutionRepresentative);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return new RemoveInstitutionRepresentativePayload(institutionRepresentative);
        }

        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        [Authorize(Policy = Configuration.AuthConfiguration.WritePolicy)]
        public async Task<ChangeInstitutionRepresentativeRolePayload> ChangeInstitutionRepresentativeRoleAsync(
            ChangeInstitutionRepresentativeRoleInput input,
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
            [ScopedService] UserManager<Data.User> userManager,
            [ScopedService] Data.ApplicationDbContext context,
            CancellationToken cancellationToken
            )
        {
            if (!await InstitutionAuthorization.IsAuthorizedToManageRepresentatives(
                 claimsPrincipal,
                 input.InstitutionId,
                 userManager,
                 context,
                 cancellationToken
                 ).ConfigureAwait(false)
               )
            {
                return new ChangeInstitutionRepresentativeRolePayload(
                    new ChangeInstitutionRepresentativeRoleError(
                      ChangeInstitutionRepresentativeRoleErrorCode.UNAUTHORIZED,
                      "You are not authorized to change institution representative roles.",
                      Array.Empty<string>()
                      )
                      );
            }
            var errors = new List<ChangeInstitutionRepresentativeRoleError>();
            if (!await context.Institutions.AsQueryable()
                .Where(i => i.Id == input.InstitutionId)
                .AnyAsync(cancellationToken)
                .ConfigureAwait(false)
            )
            {
                errors.Add(
                    new ChangeInstitutionRepresentativeRoleError(
                      ChangeInstitutionRepresentativeRoleErrorCode.UNKNOWN_INSTITUTION,
                      "Unknown institution.",
                      new[] { nameof(input), nameof(input.InstitutionId).FirstCharToLower() }
                      )
                );
            }
            if (!await context.Users.AsQueryable()
                .Where(u => u.Id == input.UserId)
                .AnyAsync(cancellationToken)
                .ConfigureAwait(false)
                )
            {
                errors.Add(
                    new ChangeInstitutionRepresentativeRoleError(
                      ChangeInstitutionRepresentativeRoleErrorCode.UNKNOWN_USER,
                      "Unknown user.",
                      new[] { nameof(input), nameof(input.UserId).FirstCharToLower() }
                      )
                );
            }
            if (errors.Count is not 0)
            {
                return new ChangeInstitutionRepresentativeRolePayload(errors.AsReadOnly());
            }
            var institutionRepresentative =
                await context.InstitutionRepresentatives.AsQueryable()
                .Where(r =>
                       r.InstitutionId == input.InstitutionId
                    && r.UserId == input.UserId
                    )
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
            if (institutionRepresentative is null)
            {
                return new ChangeInstitutionRepresentativeRolePayload(
                    new ChangeInstitutionRepresentativeRoleError(
                      ChangeInstitutionRepresentativeRoleErrorCode.UNKNOWN_REPRESENTATIVE,
                      "Unknown representative.",
                      new[] { nameof(input) }
                      )
                );
            }
            if (input.NewRole != Enumerations.InstitutionRepresentativeRole.OWNER
                && institutionRepresentative.Role == Enumerations.InstitutionRepresentativeRole.OWNER
                && !await ExistsOtherInstitutionOwner(
                        institutionRepresentative.InstitutionId,
                        institutionRepresentative.UserId,
                        context,
                        cancellationToken
                ).ConfigureAwait(false)
                )
            {
                return new ChangeInstitutionRepresentativeRolePayload(
                    new ChangeInstitutionRepresentativeRoleError(
                      ChangeInstitutionRepresentativeRoleErrorCode.LAST_OWNER,
                      "Cannot downgrade last owner.",
                      new[] { nameof(input) }
                      )
                );
            }
            institutionRepresentative.Role = input.NewRole;
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return new ChangeInstitutionRepresentativeRolePayload(institutionRepresentative);
        }

        private static async Task<bool> ExistsOtherInstitutionOwner(
            Guid institutionId,
            Guid userId,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
        )
        {
            return await context.InstitutionRepresentatives.AsQueryable()
                .Where(r =>
                    r.InstitutionId == institutionId
                    && r.UserId != userId
                    && r.Role == Enumerations.InstitutionRepresentativeRole.OWNER
                    )
                .AnyAsync(cancellationToken)
                .ConfigureAwait(false);
        }
    }
}