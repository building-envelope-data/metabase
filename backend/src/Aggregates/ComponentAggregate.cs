// Inspired by https://jasperfx.github.io/marten/documentation/scenarios/aggregates_events_repositories/

using System;
using System.Collections.Generic;
using Icon.Infrastructure.Aggregate;
using Events = Icon.Events;

namespace Icon.Aggregates
{
    public sealed class ComponentAggregate : EventSourcedAggregate
    {
        public ComponentInformationAggregateData Information { get; set; }

        public ComponentAggregate() { }

        private void Apply(Marten.Events.Event<Events.ComponentCreated> @event)
        {
            ApplyMeta(@event);
            var data = @event.Data;
            Id = data.ComponentId;
            Information = ComponentInformationAggregateData.From(data.Information);
        }

        public Models.Component ToModel()
        {
            return new Models.Component(
                id: Id,
                information: Information.ToModel(),
                timestamp: Timestamp
                );
        }
    }
}