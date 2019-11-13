using System;
using System.Linq;
using System.Collections.Generic;
using Marten;
using Marten.Linq;
using System.Threading.Tasks;
using Icon.Infrastructure.Event;
using CancellationToken = System.Threading.CancellationToken;
using ErrorCodes = Icon.ErrorCodes;
using HotChocolate;
using CSharpFunctionalExtensions;

namespace Icon.Infrastructure.Aggregate
{
    public sealed class AggregateRepository : IAggregateRepository
    {
        private readonly IDocumentStore _store;
        private readonly IEventBus _eventBus;

        public AggregateRepository(IDocumentStore store, IEventBus eventBus)
        {
            _store = store;
            _eventBus = eventBus;
        }

        // For the different kinds of Marten sessions see http://jasperfx.github.io/marten/documentation/troubleshoot/
        public IAggregateRepositorySession OpenSession()
        {
            return new AggregateRepositorySession(_store.OpenSession(), _eventBus);
        }

        public IAggregateRepositoryReadOnlySession OpenReadOnlySession()
        {
            // TODO We sould like to use `QuerySession` here, which however does
            // not provide access to an `IEventStore`.
            return new AggregateRepositoryReadOnlySession(_store.OpenSession());
        }
    }
}