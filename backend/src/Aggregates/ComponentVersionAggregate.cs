using System;
using System.Collections.Generic;
using Icon.Infrastructure.Aggregate;
using Marten.Schema;
using Events = Icon.Events;

namespace Icon.Aggregates
{
    public sealed class ComponentVersionAggregate
      : EventSourcedAggregate
    {
        [ForeignKey(typeof(ComponentAggregate))]
        public Guid ComponentId { get; set; }

        public ComponentInformationAggregateData Information { get; set; }

        public ComponentVersionAggregate() { }

        private void Apply(Marten.Events.Event<Events.ComponentVersionCreated> @event)
        {
            ApplyMeta(@event);
            var data = @event.Data;
            Id = data.ComponentVersionId;
            ComponentId = data.ComponentId;
            Information = ComponentInformationAggregateData.From(data.Information);
        }

        public Models.ComponentVersion ToModel()
        {
            return new Models.ComponentVersion(
              id: Id,
              componentId: ComponentId,
              information: Information.ToModel(),
              timestamp: Timestamp
            );
        }
    }
}