using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GreenDonut;
using HotChocolate.DataLoader;
using Guid = System.Guid;

namespace Metabase.GraphQl.Users
{
  public class UserByIdDataLoader
    : BatchDataLoader<Guid, Data.User>
    {
        private readonly IDbContextFactory<Data.ApplicationDbContext> _dbContextFactory;

        public UserByIdDataLoader(
            IBatchScheduler batchScheduler,
            IDbContextFactory<Data.ApplicationDbContext> dbContextFactory
            )
            : base(batchScheduler)
        {
            _dbContextFactory = dbContextFactory;
        }

        protected override async Task<IReadOnlyDictionary<Guid, Data.User>> LoadBatchAsync(
            IReadOnlyList<Guid> ids,
            CancellationToken cancellationToken
            )
        {
            await using Data.ApplicationDbContext dbContext =
                _dbContextFactory.CreateDbContext();
            return await dbContext.Users
                .Where(entity => ids.Contains(entity.Id))
                .ToDictionaryAsync(entity => entity.Id, cancellationToken);
        }
    }
}
