using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Errors = Infrastructure.Errors;
using Guid = System.Guid;
using Validatable = Infrastructure.Validatable;

namespace Infrastructure.Events
{
    public abstract class Event
      : Validatable, IEvent
    {
        public Guid CreatorId { get; set; }

#nullable disable
        protected Event() { }
#nullable enable

        protected Event(Guid creatorId)
        {
            CreatorId = creatorId;
        }

        public override Result<bool, Errors> Validate()
        {
            return ValidateNonEmpty(CreatorId, nameof(CreatorId));
        }
    }
}