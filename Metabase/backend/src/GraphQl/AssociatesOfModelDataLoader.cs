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
    public abstract class AssociatesOfModelDataLoader<TAssociateGraphQlObject, TModel, TAssociationModel, TAssociateModel, TQuery>
      : OurDataLoaderBase<TimestampedId, IReadOnlyList<TAssociateGraphQlObject>>
      where TQuery : IQuery<IEnumerable<Result<IEnumerable<Result<TAssociateModel, Errors>>, Errors>>>
    {
        private readonly Func<IReadOnlyList<TimestampedId>, Result<TQuery, Errors>> _newQuery;
        private readonly Func<TAssociateModel, Timestamp, TAssociateGraphQlObject> _mapAssociateModelToGraphQlObject;

        protected AssociatesOfModelDataLoader(
            Func<IReadOnlyList<TimestampedId>, Result<TQuery, Errors>> newQuery,
            Func<TAssociateModel, Timestamp, TAssociateGraphQlObject> mapAssociateModelToGraphQlObject,
            IQueryBus queryBus
            )
          : base(queryBus)
        {
            _newQuery = newQuery;
            _mapAssociateModelToGraphQlObject = mapAssociateModelToGraphQlObject;
        }

        protected override async Task<IReadOnlyList<GreenDonut.Result<IReadOnlyList<TAssociateGraphQlObject>>>> FetchAsync(
            IReadOnlyList<TimestampedId> timestampedIds,
            CancellationToken cancellationToken
            )
        {
            var query =
              ResultHelpers.HandleFailure(
                  _newQuery(timestampedIds)
                  );
            var results =
              await QueryBus.Send<
                  TQuery,
                  IEnumerable<Result<IEnumerable<Result<TAssociateModel, Errors>>, Errors>>
               >(query).ConfigureAwait(false);
            return ResultHelpers.ToDataLoaderResultsX<TAssociateGraphQlObject>(
                timestampedIds.Zip(results, (timestampedId, result) =>
                  result.Map(associateResults =>
                    associateResults.Select(associateResult =>
                      associateResult.Map(n => _mapAssociateModelToGraphQlObject(n, timestampedId.Timestamp))
                      )
                    )
                  )
                );
        }
    }
}