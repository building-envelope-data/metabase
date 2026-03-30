using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using GreenDonut.Data;
using Metabase.Data;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Components;

public sealed class ComponentDataLoaders
: DataLoaders
{
    [DataLoader]
    public static ValueTask<IReadOnlyDictionary<Guid, Component>> GetComponentByIdAsync(
        IReadOnlyList<Guid> ids,
        QueryContext<Component> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
    {
        return GetEntityByIdAsync(
            ids,
            databaseContext => databaseContext.Components,
            queryContext,
            databaseContextFactory,
            cancellationToken
        );
    }

    [DataLoader]
    public static ValueTask<IReadOnlyDictionary<Guid, ComponentManufacturer[]>> GetComponentManufacturersByComponentIdAsync(
        IReadOnlyList<Guid> ids,
        QueryContext<ComponentManufacturer> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
    {
        return GetAssociationsByOneIdAsync(
            ids,
            (databaseContext) => databaseContext.ComponentManufacturers.Where(_ => !_.Pending),
            _ => _.ComponentId,
            _ => _.InstitutionId,
            queryContext,
            databaseContextFactory,
            cancellationToken
        );
    }

    [DataLoader]
    public static ValueTask<IReadOnlyDictionary<Guid, ComponentManufacturer[]>> GetPendingComponentManufacturersByComponentIdAsync(
        IReadOnlyList<Guid> ids,
        QueryContext<ComponentManufacturer> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
    {
        return GetAssociationsByOneIdAsync(
            ids,
            (databaseContext) => databaseContext.ComponentManufacturers.Where(_ => _.Pending),
            _ => _.ComponentId,
            _ => _.InstitutionId,
            queryContext,
            databaseContextFactory,
            cancellationToken
        );
    }

    [DataLoader]
    public static ValueTask<IReadOnlyDictionary<Guid, ComponentAssembly[]>> GetComponentPartOfByComponentIdAsync(
        IReadOnlyList<Guid> ids,
        QueryContext<ComponentAssembly> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
    {
        return GetAssociationsByOneIdAsync(
            ids,
            (databaseContext) => databaseContext.ComponentAssemblies,
            _ => _.PartComponentId,
            _ => _.AssembledComponentId,
            queryContext,
            databaseContextFactory,
            cancellationToken
        );
    }

    [DataLoader]
    public static ValueTask<IReadOnlyDictionary<Guid, ComponentAssembly[]>> GetComponentAssembledOfByComponentIdAsync(
        IReadOnlyList<Guid> ids,
        QueryContext<ComponentAssembly> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
    {
        return GetAssociationsByOneIdAsync(
            ids,
            (databaseContext) => databaseContext.ComponentAssemblies,
            _ => _.AssembledComponentId,
            _ => _.PartComponentId,
            queryContext,
            databaseContextFactory,
            cancellationToken
        );
    }

    [DataLoader]
    public static ValueTask<IReadOnlyDictionary<Guid, ComponentVariant[]>> GetComponentVariantOfByComponentIdAsync(
        IReadOnlyList<Guid> ids,
        QueryContext<ComponentVariant> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
    {
        return GetAssociationsByOneIdAsync(
            ids,
            (databaseContext) => databaseContext.ComponentVariants,
            _ => _.ToComponentId,
            _ => _.OfComponentId,
            queryContext,
            databaseContextFactory,
            cancellationToken
        );
    }

    [DataLoader]
    public static ValueTask<IReadOnlyDictionary<Guid, ComponentConcretizationAndGeneralization[]>> GetComponentConcretizationsByComponentIdAsync(
        IReadOnlyList<Guid> ids,
        QueryContext<ComponentConcretizationAndGeneralization> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
    {
        return GetAssociationsByOneIdAsync(
            ids,
            (databaseContext) => databaseContext.ComponentConcretizationAndGeneralizations,
            _ => _.GeneralComponentId,
            _ => _.ConcreteComponentId,
            queryContext,
            databaseContextFactory,
            cancellationToken
        );
    }

    [DataLoader]
    public static ValueTask<IReadOnlyDictionary<Guid, ComponentConcretizationAndGeneralization[]>> GetComponentGeneralizationsByComponentIdAsync(
        IReadOnlyList<Guid> ids,
        QueryContext<ComponentConcretizationAndGeneralization> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
    {
        return GetAssociationsByOneIdAsync(
            ids,
            (databaseContext) => databaseContext.ComponentConcretizationAndGeneralizations,
            _ => _.ConcreteComponentId,
            _ => _.GeneralComponentId,
            queryContext,
            databaseContextFactory,
            cancellationToken
        );
    }
}