using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using GreenDonut.Data;
using Metabase.Data;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Methods;

public sealed class MethodDataLoaders
: DataLoaders
{
    [DataLoader]
    public static ValueTask<IReadOnlyDictionary<Guid, Method>> GetMethodByIdAsync(
        IReadOnlyList<Guid> ids,
        QueryContext<Method> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
    {
        return GetEntityByIdAsync(
            ids,
            databaseContext => databaseContext.Methods,
            queryContext,
            databaseContextFactory,
            cancellationToken
        );
    }

    [DataLoader]
    public static ValueTask<IReadOnlyDictionary<Guid, InstitutionMethodDeveloper[]>> GetInstitutionMethodDevelopersByMethodIdAsync(
        IReadOnlyList<Guid> ids,
        QueryContext<InstitutionMethodDeveloper> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
    {
        return GetAssociationsByOneIdAsync(
            ids,
            (databaseContext) => databaseContext.InstitutionMethodDevelopers.Where(_ => !_.Pending),
            _ => _.MethodId,
            _ => _.InstitutionId,
            queryContext,
            databaseContextFactory,
            cancellationToken
        );
    }

    [DataLoader]
    public static ValueTask<IReadOnlyDictionary<Guid, InstitutionMethodDeveloper[]>> GetPendingInstitutionMethodDevelopersByMethodIdAsync(
        IReadOnlyList<Guid> ids,
        QueryContext<InstitutionMethodDeveloper> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
    {
        return GetAssociationsByOneIdAsync(
            ids,
            (databaseContext) => databaseContext.InstitutionMethodDevelopers.Where(_ => _.Pending),
            _ => _.MethodId,
            _ => _.InstitutionId,
            queryContext,
            databaseContextFactory,
            cancellationToken
        );
    }

    [DataLoader]
    public static ValueTask<IReadOnlyDictionary<Guid, UserMethodDeveloper[]>> GetUserMethodDevelopersByMethodIdAsync(
        IReadOnlyList<Guid> ids,
        QueryContext<UserMethodDeveloper> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
    {
        return GetAssociationsByOneIdAsync(
            ids,
            (databaseContext) => databaseContext.UserMethodDevelopers.Where(_ => !_.Pending),
            _ => _.MethodId,
            _ => _.UserId,
            queryContext,
            databaseContextFactory,
            cancellationToken
        );
    }

    [DataLoader]
    public static ValueTask<IReadOnlyDictionary<Guid, UserMethodDeveloper[]>> GetPendingUserMethodDevelopersByMethodIdAsync(
        IReadOnlyList<Guid> ids,
        QueryContext<UserMethodDeveloper> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
    {
        return GetAssociationsByOneIdAsync(
            ids,
            (databaseContext) => databaseContext.UserMethodDevelopers.Where(_ => _.Pending),
            _ => _.MethodId,
            _ => _.UserId,
            queryContext,
            databaseContextFactory,
            cancellationToken
        );
    }
}