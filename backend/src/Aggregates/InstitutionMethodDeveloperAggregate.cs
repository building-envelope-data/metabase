using System;
using Marten.Schema;

namespace Icon.Aggregates
{
    public sealed class InstitutionMethodDeveloperAggregate
      : MethodDeveloperAggregate
    {
        [ForeignKey(typeof(InstitutionAggregate))]
        public override Guid StakeholderId { get; set; }

#nullable disable
        public InstitutionMethodDeveloperAggregate() { }
#nullable enable

        private void Apply(Marten.Events.Event<Events.InstitutionMethodDeveloperAdded> @event)
        {
            ApplyMeta(@event);
            ApplyData(@event.Data);
        }

        private void Apply(Marten.Events.Event<Events.InstitutionMethodDeveloperRemoved> @event)
        {
            ApplyDeleted(@event);
        }
    }
}