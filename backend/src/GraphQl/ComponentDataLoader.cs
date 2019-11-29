using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using HotChocolate;
using Icon.Infrastructure.Query;
using System.Linq;
using HotChocolate.Resolvers;
using CancellationToken = System.Threading.CancellationToken;
using CSharpFunctionalExtensions;
using IError = HotChocolate.IError;

namespace Icon.GraphQl
{
    public class ComponentDataLoader
      : OurDataLoaderBase<ValueObjects.TimestampedId, Component>
    {
        public ComponentDataLoader(IQueryBus queryBus)
          : base(new GreenDonut.DataLoaderOptions<ValueObjects.TimestampedId>(), queryBus)
        {
        }

        // TODO Use https://github.com/ChilliCream/hotchocolate/blob/master/src/DataLoader/Core/Result.cs to pass errors or values around!
        // We could also use the simple batch data loader: https://hotchocolate.io/docs/dataloaders#batch-dataloader
        protected override async Task<IReadOnlyList<GreenDonut.Result<Component>>> FetchAsync(
            IReadOnlyList<ValueObjects.TimestampedId> timestampedIds,
            CancellationToken cancellationToken)
        {
            var query =
              ResultHelpers.HandleFailure(
                  Queries.GetComponentBatch.From(timestampedIds)
                  );
            var componentResults =
              await QueryBus.Send<
                  Queries.GetComponentBatch,
                  IEnumerable<Result<Models.Component, Errors>>
               >(query);
            return ResultHelpers.ToDataLoaderResults<Component>(
                timestampedIds.Zip(
                  componentResults,
                  (timestampedId, result) =>
                    result.Map(m => Component.FromModel(m, timestampedId.Timestamp))
                  )
                );
        }
    }
}