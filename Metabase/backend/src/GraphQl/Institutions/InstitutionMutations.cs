using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Metabase.GraphQl.Institutions
{
    [ExtendObjectType(Name = nameof(Mutation))]
    public sealed class InstitutionMutations
    {
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [Authorize]
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
                      new[] { "input", "ownerIds" }
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
        [Authorize]
        public async Task<AddInstitutionRepresentativePayload> AddInstitutionRepresentativeAsync(
            AddInstitutionRepresentativeInput input,
            [ScopedService] Data.ApplicationDbContext context,
            CancellationToken cancellationToken
            )
        {
            var errors = new List<AddInstitutionRepresentativeError>();
            var isInstitutionExistent =
                await context.Institutions
                .Where(i => i.Id == input.InstitutionId)
                .AnyAsync(cancellationToken)
                .ConfigureAwait(false);
            if (!isInstitutionExistent)
            {
                errors.Add(
                    new AddInstitutionRepresentativeError(
                      AddInstitutionRepresentativeErrorCode.UNKNOWN_INSTITUTION,
                      "Unknown institution.",
                      new[] { "input", "institutionId" }
                      )
                );
            }
            var isUserExistent =
                await context.Users
                .Where(u => u.Id == input.UserId)
                .AnyAsync(cancellationToken)
                .ConfigureAwait(false);
            if (!isUserExistent)
            {
                errors.Add(
                    new AddInstitutionRepresentativeError(
                      AddInstitutionRepresentativeErrorCode.UNKNOWN_USER,
                      "Unknown user.",
                      new[] { "input", "userId" }
                      )
                );
            }
            if (isInstitutionExistent && isUserExistent)
            {
                var isInstitutionRepresentativeExistent =
                    await context.InstitutionRepresentatives
                    .Where(r =>
                           r.InstitutionId == input.InstitutionId
                        && r.UserId == input.UserId
                        )
                    .AnyAsync(cancellationToken)
                    .ConfigureAwait(false);
                if (isInstitutionRepresentativeExistent)
                {
                    errors.Add(
                        new AddInstitutionRepresentativeError(
                          AddInstitutionRepresentativeErrorCode.DUPLICATE,
                          "Institution representative already exists.",
                          new[] { "input" }
                          )
                    );
                }
            }
            if (errors.Count is not 0)
            {
                return new AddInstitutionRepresentativePayload(errors.AsReadOnly());
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
    }
}