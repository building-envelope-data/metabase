// Inspired by https://jasperfx.github.io/marten/documentation/scenarios/aggregates_events_repositories/

using System;
using System.Linq;
using System.Collections.Generic;
using Marten;
using System.Threading.Tasks;
using Icon.Infrastructure.Event;
using CancellationToken = System.Threading.CancellationToken;

namespace Icon.Infrastructure.Aggregate {
public sealed class AggregateRepository : IAggregateRepository
{
    private readonly IDocumentStore _store;
    private readonly IEventBus _eventBus;

    public AggregateRepository(IDocumentStore store, IEventBus eventBus)
    {
        _store = store;
        _eventBus = eventBus;
    }

    public async Task<T> Store<T>(T aggregate, CancellationToken cancellationToken = default(CancellationToken)) where T : IEventSourcedAggregate
    {
        using (var session = _store.OpenSession())
        {
            // Take non-persisted events, push them to the event stream, indexed by the aggregate ID
            var events = aggregate.GetUncommittedEvents().ToArray();
            session.Events.Append(aggregate.Id, aggregate.Version, events, cancellationToken);
            await session.SaveChangesAsync();
            await _eventBus.Publish(events);
        }
        // Once successfully persisted, clear events from list of uncommitted events
        aggregate.ClearUncommittedEvents();
        return aggregate;
    }

    public async Task<T> Load<T>(Guid id, int? version = null, CancellationToken cancellationToken = default(CancellationToken)) where T : IEventSourcedAggregate, new()
    {
        using (var session = _store.LightweightSession())
        {
          T aggregate = default(T); // TODO ...
            /* var aggregate = await session.Events.AggregateStreamAsync<T>(id, version ?? 0, default(DateTime), default(T), cancellationToken); */
            return aggregate ?? throw new InvalidOperationException($"No aggregate by id {id.ToString()}.");
        }
    }

    public async Task<IEnumerable<T>> LoadAll<T>(CancellationToken cancellationToken = default(CancellationToken)) where T : IEventSourcedAggregate, new()
    {
        using (var session = _store.QuerySession())
        {
          // https://jasperfx.github.io/marten/documentation/documents/querying/async/
            return await session.Query<T>().ToListAsync(cancellationToken);
        }
    }
}
}
