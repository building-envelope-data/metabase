// Inspired by https://jasperfx.github.io/marten/documentation/scenarios/aggregates_events_repositories/

using CSharpFunctionalExtensions;
using Icon;
using System;
using System.Collections.Generic;
using Icon.Infrastructure.Aggregate;
using Events = Icon.Events;
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