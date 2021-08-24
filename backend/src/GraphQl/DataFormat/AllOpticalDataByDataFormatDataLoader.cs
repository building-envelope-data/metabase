using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate.DataLoader;
using Metabase.GraphQl.DataX;

namespace Metabase.GraphQl.DataFormat
{
    internal sealed class AllOpticalDataByDataFormatDataLoader
      : BatchDataLoader<(Data.DataFormat DataFormat, OpticalDataPropositionInput Where, DateTime? Timestamp, string? Locale, uint? First, string? After, uint? Last, string? Before), OpticalDataConnection>
    {
        private readonly IDbContextFactory<Data.ApplicationDbContext> _dbContextFactory;

        public AllOpticalDataByDataFormatDataLoader(
            IBatchScheduler batchScheduler,
            IDbContextFactory<Data.ApplicationDbContext> dbContextFactory
            )
             : base(batchScheduler)
        {
            _dbContextFactory = dbContextFactory;
        }

        protected override
            Task<IReadOnlyDictionary<(Data.DataFormat DataFormat, OpticalDataPropositionInput Where, DateTime? Timestamp, string? Locale, uint? First, string? After, uint? Last, string? Before), OpticalDataConnection>>
            LoadBatchAsync(
                IReadOnlyList<(Data.DataFormat DataFormat, OpticalDataPropositionInput Where, DateTime? Timestamp, string? Locale, uint? First, string? After, uint? Last, string? Before)> keys,
                CancellationToken cancellationToken
            )
        {
            await using var dbContext =
                _dbContextFactory.CreateDbContext();
            return await _getQueryable(dbContext).AsQueryable()
                .Where(entity => ids.Contains(entity.Id))
                .ToDictionaryAsync(
                    entity => entity.Id,
                    entity => (TEntity?)entity,
                    cancellationToken
                    )
                .ConfigureAwait(false);
            keys
            .GroupBy(key => (key.Where, key.Timestamp, key.Locale, key.First, key.After, key.Last, key.Before))
            .;
        }
    }
}