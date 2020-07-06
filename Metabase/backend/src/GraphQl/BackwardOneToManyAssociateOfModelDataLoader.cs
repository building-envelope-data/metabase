using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HotChocolate;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;
using CancellationToken = System.Threading.CancellationToken;
using Errors = Infrastructure.Errors;

namespace Metabase.GraphQl
{
    public class BackwardOneToManyAssociateOfModelDataLoader<TGraphQlObject, TAssociateModel, TAssociationModel, TModel>
      : OurDataLoaderBase<TimestampedId, TGraphQlObject>
    {
        private readonly Func<TModel, Timestamp, TGraphQlObject> _mapModelToGraphQlObject;

        public BackwardOneToManyAssociateOfModelDataLoader(
            Func<TModel, Timestamp, TGraphQlObject> mapModelToGraphQlObject,
            IQueryBus queryBus
            )
          : base(queryBus)
        {
            _mapModelToGraphQlObject = mapModelToGraphQlObject;
        }

        protected override async Task<IReadOnlyList<GreenDonut.Result<TGraphQlObject>>> FetchAsync(
            IReadOnlyList<TimestampedId> timestampedIds,
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