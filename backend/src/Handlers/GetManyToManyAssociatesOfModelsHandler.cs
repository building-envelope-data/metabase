using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Icon.Events;
using Icon.Infrastructure.Aggregate;
using Icon.Infrastructure.Command;
using Icon.Infrastructure.Query;
using Marten;
using Marten.Linq.MatchesSql;
using Aggregates = Icon.Aggregates;
using CancellationToken = System.Threading.CancellationToken;
using Events = Icon.Events;
using Guid = System.Guid;
using IError = HotChocolate.IError;
using Models = Icon.Models;
using Queries = Icon.Queries;

namespace Icon.Handlers
{
    public abstract class GetManyToManyAssociatesOfModelsHandler<TModel, TAssociationModel, TAssociateModel, TAggregate, TAssociationAggregate, TAssociateAggregate>
      : IQueryHandler<Queries.GetManyToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>, IEnumerable<Result<IEnumerable<Result<TAssociateModel, Errors>>, Errors>>>
      where TAssociationModel : Models.IManyToManyAssociation
      where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
      where TAssociationAggregate : class, IEventSourcedAggregate, IConvertible<TAssociationModel>, Aggregates.IManyToManyAssociationAggregate, new()
      where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
    {
        public static async Task<IEnumerable<Result<IEnumerable<Result<TAssociateModel, Errors>>, Errors>>> Do(
            IAggregateRepositoryReadOnlySession session,
            IEnumerable<ValueObjects.TimestampedId> timestampedIds,
            Func<TAssociationModel, ValueObjects.Id> getAssociateId,
            Func<IAggregateRepositoryReadOnlySession, IEnumerable<ValueObjects.TimestampedId>, CancellationToken, Task<IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>>> getAssociations,
            CancellationToken cancellationToken
            )
        {
            var results =
              await getAssociations(
                session,
                timestampedIds,
                cancellationToken
                )
              .ConfigureAwait(false);
            var timestampsAndAssociatesIds =
              timestampedIds
              .Zip(results, (timestampedId, result) => (
                    timestampedId.Timestamp,
                    result.IsSuccess
                    ? result.Value.Where(r => r.IsSuccess).Select(r => getAssociateId(r.Value))
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

        private readonly IAggregateRepository _repository;
        private readonly Func<TAssociationModel, ValueObjects.Id> _getAssociateId;
        private readonly Func<IAggregateRepositoryReadOnlySession, IEnumerable<ValueObjects.TimestampedId>, CancellationToken, Task<IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>>> _getAssociations;

        public GetManyToManyAssociatesOfModelsHandler(
            IAggregateRepository repository,
            Func<TAssociationModel, ValueObjects.Id> getAssociateId,
            Func<IAggregateRepositoryReadOnlySession, IEnumerable<ValueObjects.TimestampedId>, CancellationToken, Task<IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>>> getAssociations
            )
        {
            _repository = repository;
            _getAssociateId = getAssociateId;
            _getAssociations = getAssociations;
        }

        public async Task<IEnumerable<Result<IEnumerable<Result<TAssociateModel, Errors>>, Errors>>> Handle(
            Queries.GetManyToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel> query,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenReadOnlySession())
            {
                return await Do(
                    session,
                    query.TimestampedIds,
                    _getAssociateId,
                    _getAssociations,
                    cancellationToken
                    )
                  .ConfigureAwait(false);
            }
        }
    }
}