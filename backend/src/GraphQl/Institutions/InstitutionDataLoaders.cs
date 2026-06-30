using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using GreenDonut.Data;
using Metabase.Data;
using Metabase.Data.OpenIdConnect;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionDataLoaders
: DataLoaders
{
    [DataLoader]
    public static ValueTask<IReadOnlyDictionary<Guid, Institution>> GetInstitutionByIdAsync(
        IReadOnlyList<Guid> ids,
        QueryContext<Institution> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
    {
        return GetEntityByIdAsync(
            ids,
            databaseContext => databaseContext.Institutions,
            queryContext,
            databaseContextFactory,
            cancellationToken
        );
    }

    [DataLoader]
    public static ValueTask<IReadOnlyDictionary<Guid, Page<ComponentManufacturer>>> GetInstitutionManufacturedComponentsByInstitutionIdAsync(
        IReadOnlyList<Guid> ids,
        PagingArguments pagingArguments,
        QueryContext<ComponentManufacturer> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
    {
        return GetAssociationsByOneIdAsync(
            ids,
            (databaseContext) => databaseContext.ComponentManufacturers.Where(_ => !_.Pending),
            _ => _.InstitutionId,
            _ => _.ComponentId,
            pagingArguments,
            queryContext,
            databaseContextFactory,
            cancellationToken
        );
    }

    [DataLoader]
    public static ValueTask<IReadOnlyDictionary<Guid, Page<ComponentManufacturer>>> GetPendingInstitutionManufacturedComponentsByInstitutionIdAsync(
        IReadOnlyList<Guid> ids,
        PagingArguments pagingArguments,
        QueryContext<ComponentManufacturer> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
    {
        return GetAssociationsByOneIdAsync(
            ids,
            (databaseContext) => databaseContext.ComponentManufacturers.Where(_ => _.Pending),
            _ => _.InstitutionId,
            _ => _.ComponentId,
            pagingArguments,
            queryContext,
            databaseContextFactory,
            cancellationToken
        );
    }

    [DataLoader]
    public static ValueTask<IReadOnlyDictionary<Guid, Page<InstitutionMethodDeveloper>>> GetInstitutionDevelopedMethodsByInstitutionIdAsync(
        IReadOnlyList<Guid> ids,
        PagingArguments pagingArguments,
        QueryContext<InstitutionMethodDeveloper> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
    {
        return GetAssociationsByOneIdAsync(
            ids,
            (databaseContext) => databaseContext.InstitutionMethodDevelopers.Where(_ => !_.Pending),
            _ => _.InstitutionId,
            _ => _.MethodId,
            pagingArguments,
            queryContext,
            databaseContextFactory,
            cancellationToken
        );
    }

    [DataLoader]
    public static ValueTask<IReadOnlyDictionary<Guid, Page<InstitutionMethodDeveloper>>> GetPendingInstitutionDevelopedMethodsByInstitutionIdAsync(
        IReadOnlyList<Guid> ids,
        PagingArguments pagingArguments,
        QueryContext<InstitutionMethodDeveloper> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
    {
        return GetAssociationsByOneIdAsync(
            ids,
            (databaseContext) => databaseContext.InstitutionMethodDevelopers.Where(_ => _.Pending),
            _ => _.InstitutionId,
            _ => _.MethodId,
            pagingArguments,
            queryContext,
            databaseContextFactory,
            cancellationToken
        );
    }

    [DataLoader]
    public static ValueTask<IReadOnlyDictionary<Guid, Database[]>> GetInstitutionOperatedDatabasesByInstitutionIdAsync(
        IReadOnlyList<Guid> ids,
        QueryContext<Database> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
    {
        return GetManyByOneIdAsync(
            ids,
            (databaseContext) => databaseContext.Databases,
            _ => _.OperatorId,
            queryContext,
            databaseContextFactory,
            cancellationToken
        );
    }

    [DataLoader]
    public static ValueTask<IReadOnlyDictionary<Guid, OpenIdConnectApplication[]>> GetInstitutionOwnedOpenIdConnectApplicationsByInstitutionIdAsync(
        IReadOnlyList<Guid> ids,
        QueryContext<OpenIdConnectApplication> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
    {
        return GetManyByOneIdAsync(
            ids,
            (databaseContext) => databaseContext.OpenIdConnectApplications,
            _ => _.OwnerId,
            queryContext,
            databaseContextFactory,
            cancellationToken
        );
    }

    [DataLoader]
    public static ValueTask<IReadOnlyDictionary<Guid, InstitutionRepresentative[]>> GetInstitutionRepresentativesByInstitutionIdAsync(
        IReadOnlyList<Guid> ids,
        QueryContext<InstitutionRepresentative> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
    {
        return GetAssociationsByOneIdAsync(
            ids,
            (databaseContext) => databaseContext.InstitutionRepresentatives.Where(_ => !_.Pending),
            _ => _.InstitutionId,
            _ => _.UserId,
            queryContext,
            databaseContextFactory,
            cancellationToken
        );
    }

    [DataLoader]
    public static ValueTask<IReadOnlyDictionary<Guid, InstitutionRepresentative[]>> GetPendingInstitutionRepresentativesByInstitutionIdAsync(
        IReadOnlyList<Guid> ids,
        QueryContext<InstitutionRepresentative> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
    {
        return GetAssociationsByOneIdAsync(
            ids,
            (databaseContext) => databaseContext.InstitutionRepresentatives.Where(_ => _.Pending),
            _ => _.InstitutionId,
            _ => _.UserId,
            queryContext,
            databaseContextFactory,
            cancellationToken
        );
    }

    [DataLoader]
    public static ValueTask<IReadOnlyDictionary<Guid, GnuPgKeyFingerprint[]>> GetGnuPgKeyFingerprintsByInstitutionIdAsync(
        IReadOnlyList<Guid> ids,
        QueryContext<GnuPgKeyFingerprint> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
    {
        return GetManyByOneIdAsync(
            ids,
            (databaseContext) => databaseContext.GnuPgKeyFingerprints,
            _ => _.InstitutionId,
            queryContext,
            databaseContextFactory,
            cancellationToken
        );
    }

    [DataLoader]
    public static ValueTask<IReadOnlyDictionary<Guid, Page<Component>>> GetInstitutionManagedComponentsByInstitutionIdAsync(
        IReadOnlyList<Guid> ids,
        PagingArguments pagingArguments,
        QueryContext<Component> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
    {
        return GetManyByOneIdAsync(
            ids,
            (databaseContext) => databaseContext.Components,
            _ => _.ManagerId,
            pagingArguments,
            queryContext,
            databaseContextFactory,
            cancellationToken
        );
    }

    [DataLoader]
    public static ValueTask<IReadOnlyDictionary<Guid, Page<DataFormat>>> GetInstitutionManagedDataFormatsByInstitutionIdAsync(
        IReadOnlyList<Guid> ids,
        PagingArguments pagingArguments,
        QueryContext<DataFormat> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
    {
        return GetManyByOneIdAsync(
            ids,
            (databaseContext) => databaseContext.DataFormats,
            _ => _.ManagerId,
            pagingArguments,
            queryContext,
            databaseContextFactory,
            cancellationToken
        );
    }

    [DataLoader]
    public static ValueTask<IReadOnlyDictionary<Guid, Page<Institution>>> GetInstitutionManagedInstitutionsByInstitutionIdAsync(
        IReadOnlyList<Guid> ids,
        PagingArguments pagingArguments,
        QueryContext<Institution> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
    {
        return GetManyByOneIdAsync(
            ids,
            (databaseContext) => databaseContext.Institutions,
            _ => _.ManagerId ?? Guid.Empty,
            pagingArguments,
            queryContext,
            databaseContextFactory,
            cancellationToken
        );
    }

    [DataLoader]
    public static ValueTask<IReadOnlyDictionary<Guid, Page<Method>>> GetInstitutionManagedMethodsByInstitutionIdAsync(
        IReadOnlyList<Guid> ids,
        PagingArguments pagingArguments,
        QueryContext<Method> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
    {
        return GetManyByOneIdAsync(
            ids,
            (databaseContext) => databaseContext.Methods,
            _ => _.ManagerId,
            pagingArguments,
            queryContext,
            databaseContextFactory,
            cancellationToken
        );
    }
}