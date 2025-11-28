using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Metabase.Authorization;
using Metabase.Configuration;
using Metabase.Data;
using Metabase.Extensions;
using Metabase.GraphQl.Users;
using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;

namespace Metabase.GraphQl.Components;

[ExtendObjectType(nameof(Mutation))]
public sealed class ComponentMutations
{
    [UseUserManager]
    [Authorize(Policy = AuthConfiguration.WritePolicy)]
    public async Task<CreateComponentPayload> CreateComponentAsync(
        CreateComponentInput input,
        ClaimsPrincipal claimsPrincipal,
        ComponentAuthorization authorization,
        ApplicationDbContext context,
        CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToCreateComponentForInstitution(
                claimsPrincipal,
                input.ManufacturerId,
                cancellationToken
            )
           )
        {
            return new CreateComponentPayload(
                new CreateComponentError(
                    CreateComponentErrorCode.UNAUTHORIZED,
                    "You are not authorized to create components for the institution.",
                    [nameof(input), nameof(input.ManufacturerId).FirstCharToLower()]
                )
            );
        }

        if (input.ComponentId is not null
            && await context.Components.AsQueryable()
                .AnyAsync(
                    x => x.Id == input.ComponentId,
                    cancellationToken
                )
           )
        {
            return new CreateComponentPayload(
                new CreateComponentError(
                    CreateComponentErrorCode.DUPLICATE_COMPONENT_ID,
                    "The component ID is already in use.",
                    [nameof(input), nameof(input.ComponentId).FirstCharToLower()]
                )
            );
        }

        if (!await context.Institutions.AsQueryable()
                .AnyAsync(
                    x => x.Id == input.ManufacturerId,
                    cancellationToken
                )
           )
        {
            return new CreateComponentPayload(
                new CreateComponentError(
                    CreateComponentErrorCode.UNKNOWN_MANUFACTURER,
                    "Unknown manufacturer",
                    [nameof(input), nameof(input.ManufacturerId).FirstCharToLower()]
                )
            );
        }

        if (input.PrimeSurface?.Reference?.Standard is not null
            && input.PrimeSurface?.Reference?.Publication is not null)
        {
            return new CreateComponentPayload(
                new CreateComponentError(
                    CreateComponentErrorCode.AMBIGUOUS_REFERENCE,
                    "Both standard and publication are non-null.",
                    [nameof(input), nameof(input.PrimeSurface).FirstCharToLower(), nameof(input.PrimeSurface.Reference).FirstCharToLower()]
                )
            );
        }

        if (input.PrimeDirection?.Reference?.Standard is not null
            && input.PrimeDirection?.Reference?.Publication is not null)
        {
            return new CreateComponentPayload(
                new CreateComponentError(
                    CreateComponentErrorCode.AMBIGUOUS_REFERENCE,
                    "Both standard and publication are non-null.",
                    [nameof(input), nameof(input.PrimeDirection).FirstCharToLower(), nameof(input.PrimeDirection.Reference).FirstCharToLower()]
                )
            );
        }

        if (input.SwitchableLayers?.Reference?.Standard is not null
            && input.SwitchableLayers?.Reference?.Publication is not null)
        {
            return new CreateComponentPayload(
                new CreateComponentError(
                    CreateComponentErrorCode.AMBIGUOUS_REFERENCE,
                    "Both standard and publication are non-null.",
                    [nameof(input), nameof(input.SwitchableLayers).FirstCharToLower(), nameof(input.SwitchableLayers.Reference).FirstCharToLower()]
                )
            );
        }

        NpgsqlRange<DateTime>? availability = input.Availability?.ToDomainModel();
        // Note that above we make sure that, for each reference, standard and publication are *not* both non-null.
        var primeSurface = input.PrimeSurface?.ToDomainModel();
        var primeDirection = input.PrimeDirection?.ToDomainModel();
        var switchableLayers = input.SwitchableLayers?.ToDomainModel();
        var component =
            input.ComponentId is null
            ? new Component(
                input.Name,
                input.Abbreviation,
                input.Description,
                availability,
                input.Categories,
                input.Extras
            )
            {
                PrimeSurface = primeSurface,
                PrimeDirection = primeDirection,
                SwitchableLayers = switchableLayers,
            }
            : new Component(
                input.ComponentId ?? Guid.Empty,
                input.Name,
                input.Abbreviation,
                input.Description,
                availability,
                input.Categories,
                input.Extras
            )
            {
                PrimeSurface = primeSurface,
                PrimeDirection = primeDirection,
                SwitchableLayers = switchableLayers,
            };

        component.ManufacturerEdges.Add(
            new ComponentManufacturer
            {
                InstitutionId = input.ManufacturerId,
                Pending = false
            }
        );
        context.Components.Add(component);
        await context.SaveChangesAsync(cancellationToken);
        return new CreateComponentPayload(component);
    }

    [UseUserManager]
    [Authorize(Policy = AuthConfiguration.WritePolicy)]
    public async Task<UpdateComponentPayload> UpdateComponentAsync(
        UpdateComponentInput input,
        ClaimsPrincipal claimsPrincipal,
        ComponentAuthorization authorization,
        ComponentManufacturerAuthorization manufacturerAuthorization,
        ApplicationDbContext context,
        CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToUpdate(
                claimsPrincipal,
                input.ComponentId,
                cancellationToken
            )
           )
        {
            return new UpdateComponentPayload(
                new UpdateComponentError(
                    UpdateComponentErrorCode.UNAUTHORIZED,
                    "You are not authorized to update the component.",
                    []
                )
            );
        }

        if (input.ManufacturerId is not null
            && !await manufacturerAuthorization.IsAuthorizedToAdd(
                    claimsPrincipal,
                    input.ComponentId,
                    cancellationToken
            )
        )
        {
            return new UpdateComponentPayload(
                    new UpdateComponentError(
                        UpdateComponentErrorCode.UNAUTHORIZED,
                        "You are not authorized to add the manufacturer to the component.",
                        [nameof(input), nameof(input.ComponentId).FirstCharToLower()]
                        )
                    );
        }

        var component =
            await context.Components.AsQueryable()
                .Where(i => i.Id == input.ComponentId)
                .Include(i => i.ManufacturerEdges)
                .SingleOrDefaultAsync(cancellationToken);
        if (component is null)
        {
            return new UpdateComponentPayload(
                new UpdateComponentError(
                    UpdateComponentErrorCode.UNKNOWN_COMPONENT,
                    "Unknown component.",
                    [nameof(input), nameof(input.ComponentId).FirstCharToLower()]
                )
            );
        }

        if (input.ManufacturerId is not null)
        {
            foreach (var manufacturerEdge in component.ManufacturerEdges)
            {
                if (!await manufacturerAuthorization.IsAuthorizedToRemove(
                        claimsPrincipal,
                        manufacturerEdge.InstitutionId,
                        cancellationToken
                    )
                )
                {
                    return new UpdateComponentPayload(
                            new UpdateComponentError(
                                UpdateComponentErrorCode.UNAUTHORIZED,
                                $"You are not authorized to remove the manufacturer {manufacturerEdge.InstitutionId} from the component.",
                                [nameof(input), nameof(input.ComponentId).FirstCharToLower()]
                                )
                            );
                }
            }

        }

        if (input.ManufacturerId is not null
                && !await context.Institutions.AsQueryable()
                    .AnyAsync(
                        x => x.Id == input.ManufacturerId,
                        cancellationToken
                )
            )
        {
            return new UpdateComponentPayload(
                new UpdateComponentError(
                    UpdateComponentErrorCode.UNKNOWN_MANUFACTURER,
                    "Unknown manufacturer",
                    [nameof(input), nameof(input.ManufacturerId).FirstCharToLower()]
                )
            );
        }

        if (input.PrimeSurface?.Reference?.Standard is not null
            && input.PrimeSurface?.Reference?.Publication is not null)
        {
            return new UpdateComponentPayload(
                new UpdateComponentError(
                    UpdateComponentErrorCode.AMBIGUOUS_REFERENCE,
                    "Both standard and publication are non-null.",
                    [nameof(input), nameof(input.PrimeSurface).FirstCharToLower(), nameof(input.PrimeSurface.Reference).FirstCharToLower()]
                )
            );
        }

        if (input.PrimeDirection?.Reference?.Standard is not null
            && input.PrimeDirection?.Reference?.Publication is not null)
        {
            return new UpdateComponentPayload(
                new UpdateComponentError(
                    UpdateComponentErrorCode.AMBIGUOUS_REFERENCE,
                    "Both standard and publication are non-null.",
                    [nameof(input), nameof(input.PrimeDirection).FirstCharToLower(), nameof(input.PrimeDirection.Reference).FirstCharToLower()]
                )
            );
        }

        if (input.SwitchableLayers?.Reference?.Standard is not null
            && input.SwitchableLayers?.Reference?.Publication is not null)
        {
            return new UpdateComponentPayload(
                new UpdateComponentError(
                    UpdateComponentErrorCode.AMBIGUOUS_REFERENCE,
                    "Both standard and publication are non-null.",
                    [nameof(input), nameof(input.SwitchableLayers).FirstCharToLower(), nameof(input.SwitchableLayers.Reference).FirstCharToLower()]
                )
            );
        }

        component.Update(
            input.Name,
            input.Abbreviation,
            input.Description,
            input.Availability?.ToDomainModel(),
            input.Categories,
            input.Extras
        );

        // Note that above we make sure that, for each reference, standard and publication are *not* both non-null.
        component.PrimeSurface = input.PrimeSurface?.ToDomainModel();
        component.PrimeDirection = input.PrimeDirection?.ToDomainModel();
        component.SwitchableLayers = input.SwitchableLayers?.ToDomainModel();

        if (input.ManufacturerId is not null)
        {
            component.ManufacturerEdges.Clear();
            component.ManufacturerEdges.Add(
                new ComponentManufacturer
                {
                    InstitutionId = input.ManufacturerId ?? Guid.Empty,
                    Pending = false
                }
            );
        }
        await context.SaveChangesAsync(cancellationToken);
        return new UpdateComponentPayload(component);
    }

    [UseUserManager]
    [Authorize(Policy = AuthConfiguration.WritePolicy)]
    public async Task<SetComponentExtrasPayload> SetComponentExtrasAsync(
        SetComponentExtrasInput input,
        ClaimsPrincipal claimsPrincipal,
        ComponentAuthorization authorization,
        ApplicationDbContext context,
        CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToUpdate(
                claimsPrincipal,
                input.ComponentId,
                cancellationToken
            )
           )
        {
            return new SetComponentExtrasPayload(
                new SetComponentExtrasError(
                    SetComponentExtrasErrorCode.UNAUTHORIZED,
                    "You are not authorized to update the component.",
                    []
                )
            );
        }
        var component =
            await context.Components.AsQueryable()
                .Where(i => i.Id == input.ComponentId)
                .SingleOrDefaultAsync(cancellationToken);
        if (component is null)
        {
            return new SetComponentExtrasPayload(
                new SetComponentExtrasError(
                    SetComponentExtrasErrorCode.UNKNOWN_COMPONENT,
                    "Unknown component.",
                    [nameof(input), nameof(input.ComponentId).FirstCharToLower()]
                )
            );
        }
        component.Update(input.Extras);
        await context.SaveChangesAsync(cancellationToken);
        return new SetComponentExtrasPayload(component);
    }
}