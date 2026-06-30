using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut.Data;
using LinqKit;
using Metabase.Data;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl;

public abstract class DataLoaders
{
    public static async ValueTask<IReadOnlyDictionary<Guid, TEntity>> GetEntityByIdAsync<TEntity>
    (
        IReadOnlyList<Guid> ids,
        Func<ApplicationDbContext, DbSet<TEntity>> getEntities,
        QueryContext<TEntity> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
        where TEntity : class, IEntity, IAuditable
    {
        await using var databaseContext = await databaseContextFactory.CreateDbContextAsync(cancellationToken);
        return await getEntities(databaseContext)
            .AsNoTrackingWithIdentityResolution()
            .Where(_ => ids.Contains(_.Id))
            .With(queryContext, Sorting.DefaultEntityOrder)
            .ToDictionaryAsync(_ => _.Id, cancellationToken);
    }

    public static async ValueTask<IReadOnlyDictionary<Guid, TMany[]>> GetManyByOneIdAsync<TMany>(
        IReadOnlyList<Guid> ids,
        Func<ApplicationDbContext, IQueryable<TMany>> getMany,
        Expression<Func<TMany, Guid>> getOneId,
        QueryContext<TMany> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
        where TMany : class, IEntity, IAuditable
    {
        await using var databaseContext = await databaseContextFactory.CreateDbContextAsync(cancellationToken);
        return
            await getMany(databaseContext)
            .AsExpandable()
            .AsNoTracking()
            .Where(_ => ids.Contains(getOneId.Invoke(_)))
            .With(queryContext, Sorting.DefaultEntityOrder)
            .GroupBy(getOneId)
            .Select(_ => new { _.Key, Items = _.ToArray() })
            .ToDictionaryAsync(_ => _.Key, _ => _.Items, cancellationToken);
    }

    public static async ValueTask<IReadOnlyDictionary<Guid, Page<TMany>>> GetManyByOneIdAsync<TMany>(
        IReadOnlyList<Guid> ids,
        Func<ApplicationDbContext, IQueryable<TMany>> getMany,
        Expression<Func<TMany, Guid>> getOneId,
        PagingArguments pagingArguments,
        QueryContext<TMany> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
        where TMany : class, IEntity, IAuditable
    {
        await using var databaseContext = await databaseContextFactory.CreateDbContextAsync(cancellationToken);
        return
            await getMany(databaseContext)
            .AsExpandable()
            .AsNoTracking()
            .Where(_ => ids.Contains(getOneId.Invoke(_)))
            .With(queryContext, Sorting.DefaultEntityOrder)
            .ToBatchPageAsync(getOneId, pagingArguments, cancellationToken);
    }

    public static async ValueTask<IReadOnlyDictionary<Guid, TAssociation[]>> GetAssociationsByOneIdAsync<TAssociation>(
        IReadOnlyList<Guid> ids,
        Func<ApplicationDbContext, IQueryable<TAssociation>> getAssociations,
        Expression<Func<TAssociation, Guid>> getOneId,
        Expression<Func<TAssociation, Guid>> getOtherId,
        QueryContext<TAssociation> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
        where TAssociation : class, IAssociation, IAuditable
    {
        await using var databaseContext = await databaseContextFactory.CreateDbContextAsync(cancellationToken);
        return
            await getAssociations(databaseContext)
            .AsExpandable()
            .AsNoTracking()
            .Where(_ => ids.Contains(getOneId.Invoke(_)))
            .With(queryContext, _ => _.AddDescending(getOneId).AddDescending(getOtherId))
            .GroupBy(getOneId)
            .Select(_ => new { _.Key, Items = _.ToArray() })
            .ToDictionaryAsync(_ => _.Key, _ => _.Items, cancellationToken);
    }

    public static async ValueTask<IReadOnlyDictionary<Guid, Page<TAssociation>>> GetAssociationsByOneIdAsync<TAssociation>(
        IReadOnlyList<Guid> ids,
        Func<ApplicationDbContext, IQueryable<TAssociation>> getAssociations,
        Expression<Func<TAssociation, Guid>> getOneId,
        Expression<Func<TAssociation, Guid>> getOtherId,
        PagingArguments pagingArguments,
        QueryContext<TAssociation> queryContext,
        IDbContextFactory<ApplicationDbContext> databaseContextFactory,
        CancellationToken cancellationToken
    )
        where TAssociation : class, IAssociation, IAuditable
    {
        await using var databaseContext = await databaseContextFactory.CreateDbContextAsync(cancellationToken);
        return
            await getAssociations(databaseContext)
            .AsExpandable()
            .AsNoTracking()
            .Where(_ => ids.Contains(getOneId.Invoke(_)))
            .With(queryContext, _ => _.AddDescending(getOneId).AddDescending(getOtherId))
            .ToBatchPageAsync(getOneId, pagingArguments, cancellationToken);
    }
}