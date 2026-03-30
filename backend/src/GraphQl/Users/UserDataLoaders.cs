using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using GreenDonut.Data;
using Metabase.Data;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Users;

public sealed class UserDataLoaders
: DataLoaders
{
    [DataLoader]
    public static ValueTask<IReadOnlyDictionary<Guid, User>> GetUserByIdAsync(
        IReadOnlyList<Guid> ids,
        QueryContext<User> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
    {
        return GetEntityByIdAsync(
            ids,
            databaseContext => databaseContext.Users,
            queryContext,
            databaseContextFactory,
            cancellationToken
        );
    }

    [DataLoader]
    public static ValueTask<IReadOnlyDictionary<Guid, GnuPgKeyFingerprint[]>> GetGnuPgKeyFingerprintsByUserIdAsync(
        IReadOnlyList<Guid> ids,
        QueryContext<GnuPgKeyFingerprint> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
    {
        return GetManyByOneIdAsync(
            ids,
            (databaseContext) => databaseContext.GnuPgKeyFingerprints,
            _ => _.UserId,
            queryContext,
            databaseContextFactory,
            cancellationToken
        );
    }

    [DataLoader]
    public static ValueTask<IReadOnlyDictionary<Guid, InstitutionRepresentative[]>> GetUserRepresentedInstitutionsByUserIdAsync(
        IReadOnlyList<Guid> ids,
        QueryContext<InstitutionRepresentative> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
    {
        return GetAssociationsByOneIdAsync(
            ids,
            (databaseContext) => databaseContext.InstitutionRepresentatives.Where(_ => !_.Pending),
            _ => _.UserId,
            _ => _.InstitutionId,
            queryContext,
            databaseContextFactory,
            cancellationToken
        );
    }

    [DataLoader]
    public static ValueTask<IReadOnlyDictionary<Guid, InstitutionRepresentative[]>> GetPendingUserRepresentedInstitutionsByUserIdAsync(
        IReadOnlyList<Guid> ids,
        QueryContext<InstitutionRepresentative> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
    {
        return GetAssociationsByOneIdAsync(
            ids,
            (databaseContext) => databaseContext.InstitutionRepresentatives.Where(_ => _.Pending),
            _ => _.UserId,
            _ => _.InstitutionId,
            queryContext,
            databaseContextFactory,
            cancellationToken
        );
    }

    [DataLoader]
    public static ValueTask<IReadOnlyDictionary<Guid, Page<UserMethodDeveloper>>> GetUserDevelopedMethodsByUserIdAsync(
        IReadOnlyList<Guid> ids,
        PagingArguments pagingArguments,
        QueryContext<UserMethodDeveloper> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
    {
        return GetAssociationsByOneIdAsync(
            ids,
            (databaseContext) => databaseContext.UserMethodDevelopers.Where(_ => !_.Pending),
            _ => _.UserId,
            _ => _.MethodId,
            pagingArguments,
            queryContext,
            databaseContextFactory,
            cancellationToken
        );
    }

    [DataLoader]
    public static ValueTask<IReadOnlyDictionary<Guid, Page<UserMethodDeveloper>>> GetPendingUserDevelopedMethodsByUserIdAsync(
        IReadOnlyList<Guid> ids,
        PagingArguments pagingArguments,
        QueryContext<UserMethodDeveloper> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
    {
        return GetAssociationsByOneIdAsync(
            ids,
            (databaseContext) => databaseContext.UserMethodDevelopers.Where(_ => _.Pending),
            _ => _.UserId,
            _ => _.MethodId,
            pagingArguments,
            queryContext,
            databaseContextFactory,
            cancellationToken
        );
    }
}