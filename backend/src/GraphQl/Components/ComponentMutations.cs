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
            var errors = new List<CreateComponentError>();
            if (!await context.Institutions.AsQueryable()
            .AnyAsync(
                x => x.Id == input.ManufacturerId,
             cancellationToken: cancellationToken
             )
            .ConfigureAwait(false)
            )
            {
                errors.Add(
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
                errors.Add(
                    new CreateComponentError(
                      CreateComponentErrorCode.UNKNOWN_FURTHER_MANUFACTURERS,
                      $"There are no institutions with identifier(s) {string.Join(", ", unknownFurtherManufacturerIds)}.",
                      new[] { nameof(input), nameof(input.FurtherManufacturerIds).FirstCharToLower() }
                      )
                );
            }
            var unknownAssembledOfIds =
                input.AssembledOfIds.Except(
                    await context.Components.AsQueryable()
                    .Where(x => input.AssembledOfIds.Contains(x.Id))
                    .Select(x => x.Id)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false)
                );
            if (unknownAssembledOfIds.Any())
            {
                errors.Add(
                    new CreateComponentError(
                      CreateComponentErrorCode.UNKNOWN_ASSEMBLED_OF_COMPONENTS,
                      $"There are no components with identifier(s) {string.Join(", ", unknownAssembledOfIds)}.",
                      new[] { nameof(input), nameof(input.AssembledOfIds).FirstCharToLower() }
                      )
                );
            }
            var unknownPartOfIds =
                input.PartOfIds.Except(
                    await context.Components.AsQueryable()
                    .Where(x => input.PartOfIds.Contains(x.Id))
                    .Select(x => x.Id)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false)
                );
            if (unknownPartOfIds.Any())
            {
                errors.Add(
                    new CreateComponentError(
                      CreateComponentErrorCode.UNKNOWN_PART_OF_COMPONENTS,
                      $"There are no components with identifier(s) {string.Join(", ", unknownPartOfIds)}.",
                      new[] { nameof(input), nameof(input.PartOfIds).FirstCharToLower() }
                      )
                );
            }
            var unknownConcretizationOfIds =
                input.ConcretizationOfIds.Except(
                    await context.Components.AsQueryable()
                    .Where(x => input.ConcretizationOfIds.Contains(x.Id))
                    .Select(x => x.Id)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false)
                );
            if (unknownConcretizationOfIds.Any())
            {
                errors.Add(
                    new CreateComponentError(
                      CreateComponentErrorCode.UNKNOWN_CONCRETIZATION_OF_COMPONENTS,
                      $"There are no components with identifier(s) {string.Join(", ", unknownConcretizationOfIds)}.",
                      new[] { nameof(input), nameof(input.ConcretizationOfIds).FirstCharToLower() }
                      )
                );
            }
            var unknownGeneralizationOfIds =
                input.GeneralizationOfIds.Except(
                    await context.Components.AsQueryable()
                    .Where(x => input.GeneralizationOfIds.Contains(x.Id))
                    .Select(x => x.Id)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false)
                );
            if (unknownGeneralizationOfIds.Any())
            {
                errors.Add(
                    new CreateComponentError(
                      CreateComponentErrorCode.UNKNOWN_GENERALIZATION_OF_COMPONENTS,
                      $"There are no components with identifier(s) {string.Join(", ", unknownGeneralizationOfIds)}.",
                      new[] { nameof(input), nameof(input.GeneralizationOfIds).FirstCharToLower() }
                      )
                );
            }
            var unknownVariantOfIds =
                input.VariantOfIds.Except(
                    await context.Components.AsQueryable()
                    .Where(x => input.VariantOfIds.Contains(x.Id))
                    .Select(x => x.Id)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false)
                );
            if (unknownVariantOfIds.Any())
            {
                errors.Add(
                    new CreateComponentError(
                      CreateComponentErrorCode.UNKNOWN_VARIANT_OF_COMPONENTS,
                      $"There are no components with identifier(s) {string.Join(", ", unknownVariantOfIds)}.",
                      new[] { nameof(input), nameof(input.VariantOfIds).FirstCharToLower() }
                      )
                );
            }
            if (errors.Count >= 1)
            {
                return new CreateComponentPayload(errors.AsReadOnly());
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
            foreach (var componentId in input.AssembledOfIds.Distinct())
            {
                component.PartEdges.Add(
                    new Data.ComponentAssembly
                    {
                        PartComponentId = componentId
                    }
                );
            }
            foreach (var componentId in input.PartOfIds.Distinct())
            {
                component.PartOfEdges.Add(
                    new Data.ComponentAssembly
                    {
                        AssembledComponentId = componentId
                    }
                );
            }
            foreach (var componentId in input.ConcretizationOfIds.Distinct())
            {
                component.GeneralizationEdges.Add(
                    new Data.ComponentConcretizationAndGeneralization
                    {
                        GeneralComponentId = componentId
                    }
                );
            }
            foreach (var componentId in input.GeneralizationOfIds.Distinct())
            {
                component.ConcretizationEdges.Add(
                    new Data.ComponentConcretizationAndGeneralization
                    {
                        ConcreteComponentId = componentId
                    }
                );
            }
            foreach (var componentId in input.VariantOfIds.Distinct())
            {
                component.VariantOfEdges.Add(
                    new Data.ComponentVariant
                    {
                        OfComponentId = componentId
                    }
                );
                component.VariantEdges.Add(
                    new Data.ComponentVariant
                    {
                        ToComponentId = componentId
                    }
                );
            }
            context.Components.Add(component);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return new CreateComponentPayload(component);
        }
    }
}