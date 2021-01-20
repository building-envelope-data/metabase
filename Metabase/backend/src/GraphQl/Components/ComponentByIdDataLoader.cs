using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GreenDonut;
using HotChocolate.DataLoader;
using Guid = System.Guid;

namespace Metabase.GraphQl.Components
{
    public sealed class ComponentByIdDataLoader
      : BatchDataLoader<Guid, Data.Component>
    {
        private readonly IDbContextFactory<Data.ApplicationDbContext> _dbContextFactory;

        public ComponentByIdDataLoader(
            IBatchScheduler batchScheduler,
            IDbContextFactory<Data.ApplicationDbContext> dbContextFactory
            )
            : base(batchScheduler)
        {
            _dbContextFactory = dbContextFactory;
        }

        protected override async Task<IReadOnlyDictionary<Guid, Data.Component>> LoadBatchAsync(
            IReadOnlyList<Guid> ids,
            CancellationToken cancellationToken
            )
        {
            await using var dbContext =
                _dbContextFactory.CreateDbContext();
            return await dbContext.Components
                .Where(entity => ids.Contains(entity.Id))
                .ToDictionaryAsync(entity => entity.Id, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
