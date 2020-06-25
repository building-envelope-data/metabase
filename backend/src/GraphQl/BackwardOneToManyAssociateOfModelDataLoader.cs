using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HotChocolate;
using Icon.Infrastructure.Query;
using CancellationToken = System.Threading.CancellationToken;

namespace Icon.GraphQl
{
    public class BackwardOneToManyAssociateOfModelDataLoader<TGraphQlObject, TAssociateModel, TAssociationModel, TModel>
      : OurDataLoaderBase<ValueObjects.TimestampedId, TGraphQlObject>
    {
        private readonly Func<TModel, ValueObjects.Timestamp, TGraphQlObject> _mapModelToGraphQlObject;

        public BackwardOneToManyAssociateOfModelDataLoader(
            Func<TModel, ValueObjects.Timestamp, TGraphQlObject> mapModelToGraphQlObject,
            IQueryBus queryBus
            )
          : base(queryBus)
        {
            _mapModelToGraphQlObject = mapModelToGraphQlObject;
        }

        protected override async Task<IReadOnlyList<GreenDonut.Result<TGraphQlObject>>> FetchAsync(
            IReadOnlyList<ValueObjects.TimestampedId> timestampedIds,
            CancellationToken cancellationToken
            )
        {
            var query =
              ResultHelpers.HandleFailure(
                  Queries.GetBackwardOneToManyAssociateOfModels<TAssociateModel, TAssociationModel, TModel>.From(timestampedIds)
                  );
            var results =
              await QueryBus.Send<
                  Queries.GetBackwardOneToManyAssociateOfModels<TAssociateModel, TAssociationModel, TModel>,
                  IEnumerable<Result<TModel, Errors>>
               >(query).ConfigureAwait(false);
            return ResultHelpers.ToDataLoaderResults<TGraphQlObject>(
                timestampedIds.Zip(results, (timestampedId, result) =>
                  result.Map(model =>
                    _mapModelToGraphQlObject(model, timestampedId.Timestamp)
                    )
                  )
                );
        }
    }
}