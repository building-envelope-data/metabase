using System;
using Marten.Schema;

namespace Icon.Aggregates
{
    public sealed class PersonMethodDeveloperAggregate
      : MethodDeveloperAggregate
    {
        [ForeignKey(typeof(PersonAggregate))]
        public override Guid StakeholderId { get; set; }

#nullable disable
        public PersonMethodDeveloperAggregate() { }
#nullable enable

        private void Apply(Marten.Events.Event<Events.PersonMethodDeveloperAdded> @event)
        {
            ApplyMeta(@event);
            ApplyData(@event.Data);
        }

        private void Apply(Marten.Events.Event<Events.PersonMethodDeveloperRemoved> @event)
        {
            ApplyDeleted(@event);
        }
    }
}