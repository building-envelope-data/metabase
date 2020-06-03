using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Errors = Icon.Errors;
using Guid = System.Guid;

namespace Icon.Events
{
    public abstract class Event
      : Validatable, IEvent
    {
        public static void EnsureValid(IEnumerable<IEvent> events)
        {
            foreach (var @event in events)
            {
                @event.EnsureValid();
            }
        }

        public Guid CreatorId { get; set; }

#nullable disable
        public Event() { }
#nullable enable

        public Event(Guid creatorId)
        {
            CreatorId = creatorId;
        }

        public override Result<bool, Errors> Validate()
        {
            return ValidateNonEmpty(CreatorId, nameof(CreatorId));
        }
    }
}