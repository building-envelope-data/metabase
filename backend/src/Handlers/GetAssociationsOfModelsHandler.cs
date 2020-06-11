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
    public abstract class GetAssociationsOfModelsHandler<TModel, TAssociationModel, TAggregate, TAssociationAggregate>
      where TAggregate : class, IEventSourcedAggregate, IConvertible<TModel>, new()
      where TAssociationAggregate : class, Aggregates.IAssociationAggregate, IConvertible<TAssociationModel>, new()
    {
        protected readonly IAggregateRepository _repository;

        public GetAssociationsOfModelsHandler(
            IAggregateRepository repository
            )
        {
            _repository = repository;
        }
    }
}