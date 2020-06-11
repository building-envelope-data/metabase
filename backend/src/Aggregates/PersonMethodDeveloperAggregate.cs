// Inspired by https://jasperfx.github.io/marten/documentation/scenarios/aggregates_events_repositories/

using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Icon;
using Icon.Infrastructure.Aggregate;
using Marten.Schema;
using Events = Icon.Events;

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