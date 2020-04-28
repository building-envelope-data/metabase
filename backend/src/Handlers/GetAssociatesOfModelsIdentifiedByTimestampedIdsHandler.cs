using Guid = System.Guid;
using System.Collections.Generic;
using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure.Command;
using Icon.Events;
using Icon.Infrastructure.Aggregate;
using Icon.Infrastructure.Query;
using Models = Icon.Models;
using Queries = Icon.Queries;
using Events = Icon.Events;
using Aggregates = Icon.Aggregates;
using System.Linq;
using Marten;
using Marten.Linq.MatchesSql;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;
using System;

namespace Icon.Handlers
{
    public abstract class GetAssociatesOfModelsIdentifiedByTimestampedIdsHandler<TModel, TAssociateModel, TAssociateAggregate>
      : IQueryHandler<Queries.GetAssociatesOfModelsIdentifiedByTimestampedIds<TModel, TAssociateModel>, IEnumerable<Result<IEnumerable<Result<TAssociateModel, Errors>>, Errors>>>
      where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
    {
        private readonly IAggregateRepository _repository;

        public GetAssociatesOfModelsIdentifiedByTimestampedIdsHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Result<IEnumerable<Result<TAssociateModel, Errors>>, Errors>>> Handle(
            Queries.GetAssociatesOfModelsIdentifiedByTimestampedIds<TModel, TAssociateModel> query,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenReadOnlySession())
            {
                return await Handle(query, session, cancellationToken);
            }
        }

        public async Task<IEnumerable<Result<IEnumerable<Result<TAssociateModel, Errors>>, Errors>>> Handle(
            Queries.GetAssociatesOfModelsIdentifiedByTimestampedIds<TModel, TAssociateModel> query,
            IAggregateRepositoryReadOnlySession session,
            CancellationToken cancellationToken
            )
        {
            // TODO Check existence of model ids (and use `Result.Failure` for non-existing ones)
            var modelIds = query.TimestampedModelIds.Select(timestampedId => timestampedId.Id);
            // TODO Use LINQs `GroupBy` once it has been implemented for Marten, see https://github.com/JasperFx/marten/issues/569
            var modelIdToAssociateIds = (await QueryAssociateIds(session, modelIds, cancellationToken)).ToLookup(
                modelIdAndAssociateId => modelIdAndAssociateId.Item1,
                modelIdAndAssociateId => modelIdAndAssociateId.Item2
                );
            var timestampsAndAssociatesIds =
              query.TimestampedModelIds
              .Select(timestampedId => (
                    timestampedId.Timestamp,
                    modelIdToAssociateIds.Contains(timestampedId.Id) ? modelIdToAssociateIds[timestampedId.Id] : Enumerable.Empty<ValueObjects.Id>()
                    )
                  );
            return (await
                session.LoadAllThatExistedBatched<TAssociateAggregate>(
                  timestampsAndAssociatesIds,
                  cancellationToken
                  )
                ).Select(results =>
                  Result.Ok<IEnumerable<Result<TAssociateModel, Errors>>, Errors>(
                    results.Select(result =>
                      result.Bind(a => a.ToModel())
                      )
                    )
                  );
        }

        protected abstract Task<IEnumerable<(ValueObjects.Id modelId, ValueObjects.Id associateId)>> QueryAssociateIds(
            IAggregateRepositoryReadOnlySession session,
            IEnumerable<ValueObjects.Id> modelIds,
            CancellationToken cancellationToken
            );
    }
}