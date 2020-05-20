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
    public abstract class GetManyToManyAssociatesOfModelsHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate>
      : IQueryHandler<Queries.GetAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>, IEnumerable<Result<IEnumerable<Result<TAssociateModel, Errors>>, Errors>>>
      where TAssociationModel : Models.IManyToManyAssociation
      where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
      where TAssociationAggregate : class, IEventSourcedAggregate, IConvertible<TAssociationModel>, Aggregates.IManyToManyAssociationAggregate, new()
      where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
    {
        private readonly IAggregateRepository _repository;
        private readonly GetManyToManyAssociationsOfModelsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate> _getAssociationsHandler;

        public GetManyToManyAssociatesOfModelsHandler(
            IAggregateRepository repository,
            GetManyToManyAssociationsOfModelsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate> getAssociationsHandler
            )
        {
            _repository = repository;
            _getAssociationsHandler = getAssociationsHandler;
        }

        public async Task<IEnumerable<Result<IEnumerable<Result<TAssociateModel, Errors>>, Errors>>> Handle(
            Queries.GetAssociatesOfModels<TModel, TAssociationModel, TAssociateModel> query,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenReadOnlySession())
            {
                return await Handle(query.TimestampedModelIds, session, cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<Result<IEnumerable<Result<TAssociateModel, Errors>>, Errors>>> Handle(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedModelIds,
            IAggregateRepositoryReadOnlySession session,
            CancellationToken cancellationToken
            )
        {
            var results =
              await _getAssociationsHandler.Handle(
                timestampedModelIds,
                session,
                cancellationToken
                )
              .ConfigureAwait(false);
            var timestampsAndAssociatesIds =
              timestampedModelIds
              .Zip(results, (timestampedId, result) => (
                    timestampedId.Timestamp,
                    result.IsSuccess
                    ? result.Value.Where(r => r.IsSuccess).Select(r => GetAssociateId(r.Value))
                    : Enumerable.Empty<ValueObjects.Id>()
                    )
                  );
            var batchedAssociateResults =
              await session.LoadAllBatched<TAssociateAggregate>(
                  timestampsAndAssociatesIds,
                  cancellationToken
                  )
              .ConfigureAwait(false);
            var associateResultsEnumerators =
              batchedAssociateResults.Select(associateResults =>
                  associateResults.GetEnumerator()
                  );
            // TODO For some reason evaluating the result lazily and not
            // forcing eager execution by calling `ToList` below, results in
            // `Zip` (and `Select`) being executed multiple times and thus in
            // the enumerators being used multiple times, which is not possible
            // because we do not reset them. Figure out why!
            return results.Zip(associateResultsEnumerators, (result, associateResultsEnumerator) =>
                result.Map(associationResults =>
                  associationResults.Select(associationResult =>
                    associationResult.Bind(association =>
                      associateResultsEnumerator.MoveNext()
                      ? associateResultsEnumerator.Current.Bind(associateResult =>
                          associateResult.ToModel()
                          )
                      : Result.Failure<TAssociateModel, Errors>(
                          Errors.One(
                            message: $"There is no more result for the assocation {association}",
                            code: ErrorCodes.NonExistentModel
                            )
                          )
                      )
                    ).ToList().AsEnumerable()
                  )
                ).ToList().AsEnumerable();
        }

        protected abstract ValueObjects.Id GetAssociateId(TAssociationModel association);
    }
}