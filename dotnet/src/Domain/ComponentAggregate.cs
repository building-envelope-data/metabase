using System;
using Icon.Infrastructure.Aggregate;
using Icon.Domain;

namespace Icon.Domain
{
    public class ComponentAggregate : EventSourcedAggregate
    {
      public static ComponentAggregate Create(Guid creatorId)
        {
            var component = new ComponentAggregate();
            var @event = new Component.Create.Event
            {
                ComponentId = Guid.NewGuid(),
                CreatorId = creatorId,
            };
            component.Apply(@event);
            component.Append(@event);
            return component;
        }

      public ComponentAggregate()
      {
      }

        public void Apply(Component.Create.Event @event)
        {
            Id = @event.ComponentId;
        }
    }
}