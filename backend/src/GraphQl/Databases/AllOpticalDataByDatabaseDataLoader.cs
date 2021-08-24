using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate.DataLoader;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Database
{
    internal sealed class AllOpticalDataByDatabaseDataLoader
      : BatchDataLoader<(Data.Database database, DataX.OpticalDataPropositionInput where, DateTime? timestamp, string? locale, uint? first, string? after, uint? last, string? before), OpticalDataConnection>
    {
        private readonly IDbContextFactory<Data.ApplicationDbContext> _dbContextFactory;

        public AllOpticalDataByDatabaseDataLoader(
            IBatchScheduler batchScheduler,
            IDbContextFactory<Data.ApplicationDbContext> dbContextFactory
            )
            : base(batchScheduler)
        {
            _dbContextFactory = dbContextFactory;
        }

        protected override
            Task<IReadOnlyDictionary<(Data.Database database, DataX.OpticalDataPropositionInput where, DateTime? timestamp, string? locale, uint? first, string? after, uint? last, string? before), OpticalDataConnection>>
            LoadBatchAsync(
                IReadOnlyList<(Data.Database database, DataX.OpticalDataPropositionInput where, DateTime? timestamp, string? locale, uint? first, string? after, uint? last, string? before)> keys,
                CancellationToken cancellationToken
            )
        {
            return (await QueryDatabase<AllOpticalDataData>(
                database,
                new GraphQL.GraphQLRequest(
                    query: await ConstructQuery(
                        new[] {
                            "DataFields.graphql",
                            "OpticalDataFields.graphql",
                            "PageInfoFields.graphql",
                            "AllOpticalData.graphql"
                        }
                    ).ConfigureAwait(false),
                    variables: new
                    {
                        where,
                        timestamp,
                        locale,
                        first,
                        after,
                        last,
                        before
                    },
                    operationName: "AllOpticalData"
                ),
                cancellationToken
            ).ConfigureAwait(false)
            )?.AllOpticalData;
        }
    }
}