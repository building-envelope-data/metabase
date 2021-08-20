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
using NpgsqlTypes;
using DateTime = System.DateTime;

namespace Metabase.GraphQl.Components
{
    [ExtendObjectType(nameof(Mutation))]
    public sealed class ComponentMutations
    {
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        [Authorize(Policy = Configuration.AuthConfiguration.WritePolicy)]
        public async Task<CreateComponentPayload> CreateComponentAsync(
            CreateComponentInput input,
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
            [ScopedService] UserManager<Data.User> userManager,
            [ScopedService] Data.ApplicationDbContext context,
            CancellationToken cancellationToken
            )
        {
            if (!await ComponentAuthorization.IsAuthorizedToCreateComponentForInstitution(
                 claimsPrincipal,
                 input.ManufacturerId,
                 userManager,
                 context,
                 cancellationToken
                 ).ConfigureAwait(false)
            )
            {
                return new CreateComponentPayload(
                    new CreateComponentError(
                      CreateComponentErrorCode.UNAUTHORIZED,
                      "You are not authorized to create components for the institution.",
                      new[] { nameof(input), nameof(input.ManufacturerId).FirstCharToLower() }
                    )
                );
            }
            if (!await context.Institutions.AsQueryable()
            .AnyAsync(
                x => x.Id == input.ManufacturerId,
             cancellationToken: cancellationToken
             )
            .ConfigureAwait(false)
            )
            {
                return new CreateComponentPayload(
                    new CreateComponentError(
                        CreateComponentErrorCode.UNKNOWN_MANUFACTURER,
                        "Unknown manufacturer",
                      new[] { nameof(input), nameof(input.ManufacturerId).FirstCharToLower() }
                    )
                );
            }
            var unknownFurtherManufacturerIds =
                input.FurtherManufacturerIds.Except(
                    await context.Institutions.AsQueryable()
                    .Where(x => input.FurtherManufacturerIds.Contains(x.Id))
                    .Select(x => x.Id)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false)
                );
            if (unknownFurtherManufacturerIds.Any())
            {
                return new CreateComponentPayload(
                    new CreateComponentError(
                      CreateComponentErrorCode.UNKNOWN_FURTHER_MANUFACTURERS,
                      $"There are no institutions with identifier(s) {string.Join(", ", unknownFurtherManufacturerIds)}.",
                      new[] { nameof(input), nameof(input.FurtherManufacturerIds).FirstCharToLower() }
                      )
                );
            }
            var component = new Data.Component(
                name: input.Name,
                abbreviation: input.Abbreviation,
                description: input.Description,
                availability:
                 input.Availability is not null
                 ? new NpgsqlRange<DateTime>(
                   input.Availability.From.GetValueOrDefault(), true, input.Availability.From is null,
                   input.Availability.To.GetValueOrDefault(), true, input.Availability.To is null
                   )
                 : null,
                categories: input.Categories
            );
            component.ManufacturerEdges.Add(
                new Data.ComponentManufacturer
                {
                    InstitutionId = input.ManufacturerId,
                    Pending = false
                }
            );
            foreach (var manufacturerId in input.FurtherManufacturerIds.Distinct())
            {
                if (manufacturerId != input.ManufacturerId)
                {
                    component.ManufacturerEdges.Add(
                        new Data.ComponentManufacturer
                        {
                            InstitutionId = manufacturerId,
                            Pending = true
                        }
                    );
                }
            }
            context.Components.Add(component);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return new CreateComponentPayload(component);
        }
    }
}