// Inspired by https://jasperfx.github.io/marten/documentation/scenarios/aggregates_events_repositories/

using Icon;
using System;
using System.Collections.Generic;
using Icon.Infrastructure.Aggregate;
using Events = Icon.Events;

namespace Icon.Aggregates
{
    public sealed class ComponentAggregate
      : EventSourcedAggregate
    {
        public ComponentInformationAggregateData Information { get; set; }

#nullable disable
        public ComponentAggregate() { }
#nullable enable

        private void Apply(Marten.Events.Event<Events.ComponentCreated> @event)
        {
            ApplyMeta(@event);
            var data = @event.Data;
            Id = data.ComponentId;
            Information = ComponentInformationAggregateData.From(data.Information);
        }

        public override bool IsValid()
        {
            return base.IsValid() && (
                IsVirgin()
                ) || (
                  !IsVirgin() &&
                  (Information?.IsValid() ?? false)
                  );
        }

        public Models.Component ToModel()
        {
            EnsureValid();
            return new Models.Component(
                id: Id,
                information: Information.ToModel(),
                timestamp: Timestamp
                );
        }
    }
}