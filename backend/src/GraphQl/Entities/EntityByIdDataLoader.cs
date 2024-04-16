using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using Microsoft.EntityFrameworkCore;
using Guid = System.Guid;

namespace Metabase.GraphQl.Entities
{
    public abstract class EntityByIdDataLoader<TEntity>
      : BatchDataLoader<Guid, TEntity?>
      where TEntity : class, Data.IEntity
    {
        private readonly IDbContextFactory<Data.ApplicationDbContext> _dbContextFactory;
        private readonly Func<Data.ApplicationDbContext, DbSet<TEntity>> _getQueryable;

        protected EntityByIdDataLoader(
            IBatchScheduler batchScheduler,
            DataLoaderOptions options,
            IDbContextFactory<Data.ApplicationDbContext> dbContextFactory,
            Func<Data.ApplicationDbContext, DbSet<TEntity>> getQueryable
            )
            : base(batchScheduler, options)
        {
            _dbContextFactory = dbContextFactory;
            _getQueryable = getQueryable;
        }

        protected override async Task<IReadOnlyDictionary<Guid, TEntity?>> LoadBatchAsync(
            IReadOnlyList<Guid> keys,
            CancellationToken cancellationToken
            )
        {
            await using var dbContext =
                _dbContextFactory.CreateDbContext();
            return await _getQueryable(dbContext).AsQueryable()
                .Where(entity => keys.Contains(entity.Id))
                .ToDictionaryAsync(
                    entity => entity.Id,
                    entity => (TEntity?)entity,
                    cancellationToken
                    )
                .ConfigureAwait(false);
        }
    }
}