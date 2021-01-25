using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GreenDonut;
using HotChocolate.DataLoader;
using Guid = System.Guid;

namespace Metabase.GraphQl.Entitys
{
    public abstract class EntityByIdDataLoader<TEntity>
      : BatchDataLoader<Guid, TEntity?>
      where TEntity : Infrastructure.Data.Entity
    {
        private readonly IDbContextFactory<Data.ApplicationDbContext> _dbContextFactory;
        private readonly Func<Data.ApplicationDbContext, DbSet<TEntity>> _getQueryable;

        protected EntityByIdDataLoader(
            IBatchScheduler batchScheduler,
            IDbContextFactory<Data.ApplicationDbContext> dbContextFactory,
            Func<Data.ApplicationDbContext, DbSet<TEntity>> getQueryable
            )
            : base(batchScheduler)
        {
            _dbContextFactory = dbContextFactory;
            _getQueryable = getQueryable;
        }

        protected override async Task<IReadOnlyDictionary<Guid, TEntity?>> LoadBatchAsync(
            IReadOnlyList<Guid> ids,
            CancellationToken cancellationToken
            )
        {
            await using var dbContext =
                _dbContextFactory.CreateDbContext();
            return await _getQueryable(dbContext)
                .Where(entity => ids.Contains(entity.Id))
                .ToDictionaryAsync(
                    entity => entity.Id,
                    entity => (TEntity?)entity,
                    cancellationToken
                    )
                .ConfigureAwait(false);
        }
    }
}