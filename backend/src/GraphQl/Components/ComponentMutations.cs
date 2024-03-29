using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Metabase.Authorization;
using Metabase.Extensions;
using Metabase.GraphQl.Common;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Components
{
    [ExtendObjectType(nameof(Mutation))]
    public sealed class ComponentMutations
    {
        [UseUserManager]
        [Authorize(Policy = Configuration.AuthConfiguration.WritePolicy)]
        public async Task<CreateComponentPayload> CreateComponentAsync(
            CreateComponentInput input,
            ClaimsPrincipal claimsPrincipal,
            [Service(ServiceKind.Resolver)] UserManager<Data.User> userManager,
            Data.ApplicationDbContext context,
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
            var component = new Data.Component(
                name: input.Name,
                abbreviation: input.Abbreviation,
                description: input.Description,
                availability:
                 input.Availability is null
                 ? null
                 : OpenEndedDateTimeRangeType.FromInput(input.Availability),
                categories: input.Categories
            );
            component.ManufacturerEdges.Add(
                new Data.ComponentManufacturer
                {
                    InstitutionId = input.ManufacturerId,
                    Pending = false
                }
            );
            context.Components.Add(component);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return new CreateComponentPayload(component);
        }

        [UseUserManager]
        [Authorize(Policy = Configuration.AuthConfiguration.WritePolicy)]
        public async Task<UpdateComponentPayload> UpdateComponentAsync(
            UpdateComponentInput input,
            ClaimsPrincipal claimsPrincipal,
            [Service(ServiceKind.Resolver)] UserManager<Data.User> userManager,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
            )
        {
            if (!await ComponentAuthorization.IsAuthorizedToUpdate(
                 claimsPrincipal,
                 input.ComponentId,
                 userManager,
                 context,
                 cancellationToken
                 ).ConfigureAwait(false)
               )
            {
                return new UpdateComponentPayload(
                    new UpdateComponentError(
                      UpdateComponentErrorCode.UNAUTHORIZED,
                      "You are not authorized to update the component.",
                      Array.Empty<string>()
                      )
                      );
            }
            var component =
                await context.Components.AsQueryable()
                .Where(i => i.Id == input.ComponentId)
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
            if (component is null)
            {
                return new UpdateComponentPayload(
                    new UpdateComponentError(
                      UpdateComponentErrorCode.UNKNOWN_COMPONENT,
                      "Unknown component.",
                      new[] { nameof(input), nameof(input.ComponentId).FirstCharToLower() }
                      )
                      );
            }
            component.Update(
                name: input.Name,
                abbreviation: input.Abbreviation,
                description: input.Description,
                availability:
                 input.Availability is null
                 ? null
                 : OpenEndedDateTimeRangeType.FromInput(input.Availability),
                categories: input.Categories
            );
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return new UpdateComponentPayload(component);
        }
    }
}