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
    public sealed class GetBackwardOneToManyAssociationOfModelsHandler<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>
      : GetOneToManyAssociationsOfModelsHandler<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate>,
        IQueryHandler<Queries.GetBackwardOneToManyAssociationOfModels<TAssociateModel, TAssociationModel>, IEnumerable<Result<TAssociationModel, Errors>>>
      where TAssociateAggregate : class, IEventSourcedAggregate, IConvertible<TAssociateModel>, new()
      where TAssociationAggregate : class, Aggregates.IOneToManyAssociationAggregate, IConvertible<TAssociationModel>, new()
      where TAssociationAddedEvent : Events.IAssociationAddedEvent
    {
        public GetBackwardOneToManyAssociationOfModelsHandler(IAggregateRepository repository)
          : base(repository)
        {
        }

        public async Task<IEnumerable<Result<TAssociationModel, Errors>>> Handle(
            Queries.GetBackwardOneToManyAssociationOfModels<TAssociateModel, TAssociationModel> query,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenReadOnlySession())
            {
                return await
                  session.GetBackwardOneToManyAssociationOfModels<TAssociateModel, TAssociationModel, TAssociateAggregate, TAssociationAggregate, TAssociationAddedEvent>(
                    query.TimestampedIds,
                    cancellationToken
                    )
                  .ConfigureAwait(false);
            }
        }
    }
}