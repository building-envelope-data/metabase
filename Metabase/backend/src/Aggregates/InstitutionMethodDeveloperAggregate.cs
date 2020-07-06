using System;
using Marten.Schema;

namespace Metabase.Aggregates
{
    public sealed class InstitutionMethodDeveloperAggregate
      : MethodDeveloperAggregate
    {
        [ForeignKey(typeof(InstitutionAggregate))]
        public override Guid StakeholderId { get; set; }

#nullable disable
        public InstitutionMethodDeveloperAggregate() { }
#nullable enable

        public void Apply(Marten.Events.Event<Events.InstitutionMethodDeveloperAdded> @event)
        {
            ApplyMeta(@event);
            ApplyData(@event.Data);
        }

        public void Apply(Marten.Events.Event<Events.InstitutionMethodDeveloperRemoved> @event)
        {
            ApplyDeleted(@event);
        }
    }
}