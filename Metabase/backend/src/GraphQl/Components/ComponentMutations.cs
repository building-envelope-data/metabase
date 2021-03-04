using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using Metabase.Extensions;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;
using DateTime = System.DateTime;
using Metabase.Authorization;

namespace Metabase.GraphQl.Components
{
    [ExtendObjectType(Name = nameof(Mutation))]
    public sealed class ComponentMutations
    {
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        [Authorize(Policy = Configuration.Auth.WritePolicy)]
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
                      Array.Empty<string>()
                    )
                );
            }
            if (!await context.Institutions
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
                    InstitutionId = input.ManufacturerId
                }
            );
            context.Components.Add(component);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return new CreateComponentPayload(component);
        }
    }
}