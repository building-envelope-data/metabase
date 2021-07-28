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

namespace Metabase.GraphQl.ComponentManufacturers
{
    [ExtendObjectType(nameof(Mutation))]
    public sealed class ComponentManufacturerMutations
    {
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        [Authorize(Policy = Configuration.AuthConfiguration.WritePolicy)]
        public async Task<ConfirmComponentManufacturerPayload> ConfirmComponentManufacturerAsync(
            ConfirmComponentManufacturerInput input,
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
            [ScopedService] UserManager<Data.User> userManager,
            [ScopedService] Data.ApplicationDbContext context,
            CancellationToken cancellationToken
            )
        {
            if (!await ComponentManufacturerAuthorization.IsAuthorizedToConfirm(
                 claimsPrincipal,
                 input.InstitutionId,
                 userManager,
                 context,
                 cancellationToken
                 ).ConfigureAwait(false)
               )
            {
                return new ConfirmComponentManufacturerPayload(
                    new ConfirmComponentManufacturerError(
                      ConfirmComponentManufacturerErrorCode.UNAUTHORIZED,
                      $"You are not authorized to confirm component manufacturer relation for institution ${input.InstitutionId}.",
                      Array.Empty<string>()
                      )
                      );
            }
            var errors = new List<ConfirmComponentManufacturerError>();
            if (!await context.Components.AsQueryable()
                .Where(u => u.Id == input.ComponentId)
                .AnyAsync(cancellationToken)
                .ConfigureAwait(false)
                )
            {
                errors.Add(
                    new ConfirmComponentManufacturerError(
                      ConfirmComponentManufacturerErrorCode.UNKNOWN_COMPONENT,
                      "Unknown component.",
                      new[] { nameof(input), nameof(input.ComponentId).FirstCharToLower() }
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
                    new ConfirmComponentManufacturerError(
                      ConfirmComponentManufacturerErrorCode.UNKNOWN_INSTITUTION,
                      "Unknown institution.",
                      new[] { nameof(input), nameof(input.InstitutionId).FirstCharToLower() }
                      )
                );
            }
            if (errors.Count is not 0)
            {
                return new ConfirmComponentManufacturerPayload(errors.AsReadOnly());
            }
            var componentManufacturer =
                await context.ComponentManufacturers.AsQueryable()
                .Where(r =>
                       r.ComponentId == input.ComponentId
                    && r.InstitutionId == input.InstitutionId
                    )
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
            if (componentManufacturer is null)
            {
                return new ConfirmComponentManufacturerPayload(
                    new ConfirmComponentManufacturerError(
                      ConfirmComponentManufacturerErrorCode.UNKNOWN_MANUFACTURER,
                      "Unknown manufacturer.",
                      new[] { nameof(input) }
                      )
                );
            }
            componentManufacturer.Pending = false;
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return new ConfirmComponentManufacturerPayload(componentManufacturer);
        }
    }
}