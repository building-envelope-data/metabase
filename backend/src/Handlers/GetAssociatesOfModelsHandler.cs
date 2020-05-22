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
    public abstract class GetAssociatesOfModelsHandler<TModel, TAssociateModel, TAggregate, TAssociateAggregate>
      where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
      where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
    {
        public GetAssociatesOfModelsHandler()
        {
        }

        public async Task<IEnumerable<Result<IEnumerable<Result<TAssociateModel, Errors>>, Errors>>> Handle(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedModelIds,
            IAggregateRepositoryReadOnlySession session,
            CancellationToken cancellationToken
            )
        {
            var modelIds =
              timestampedModelIds
              .Select(timestampedId => timestampedId.Id);
            var doModelIdsExist =
              await session.Exist<TAggregate>(
                  timestampedModelIds,
                  cancellationToken
                  )
              .ConfigureAwait(false);
            var existingModelIds =
              modelIds.Zip(doModelIdsExist)
              .Where(x => x.Item2)
              .Select(x => x.Item1);
            // TODO Use LINQs `GroupBy` once it has been implemented for Marten, see https://github.com/JasperFx/marten/issues/569
            var modelIdToAssociateIds =
              (await QueryAssociateIds(
                  session,
                  existingModelIds,
                  cancellationToken
                  )
                .ConfigureAwait(false)
                )
              .ToLookup(
                modelIdAndAssociateId => modelIdAndAssociateId.Item1,
                modelIdAndAssociateId => modelIdAndAssociateId.Item2
                );
            var timestampsAndAssociatesIds =
              timestampedModelIds
              .Select(timestampedId => (
                    timestampedId.Timestamp,
                    modelIdToAssociateIds.Contains(timestampedId.Id)
                    ? modelIdToAssociateIds[timestampedId.Id]
                    : Enumerable.Empty<ValueObjects.Id>()
                    )
                  );
            return
              (await session.LoadAllThatExistedBatched<TAssociateAggregate>(
                  timestampsAndAssociatesIds,
                  cancellationToken
                  )
                .ConfigureAwait(false)
                )
              .Zip(modelIds.Zip(doModelIdsExist), (results, modelIdAndExists) =>
                  modelIdAndExists.Item2
                  ? Result.Ok<IEnumerable<Result<TAssociateModel, Errors>>, Errors>(
                    results.Select(result =>
                      result.Bind(a => a.ToModel())
                      )
                    )
                  : Result.Failure<IEnumerable<Result<TAssociateModel, Errors>>, Errors>(
                    Errors.One(
                      message: $"There is no model with id {modelIdAndExists.Item1}",
                      code: ErrorCodes.NonExistentModel
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
