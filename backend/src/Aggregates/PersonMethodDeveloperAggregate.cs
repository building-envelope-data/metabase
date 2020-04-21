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
    }
}