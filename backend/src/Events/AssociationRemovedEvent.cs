using CSharpFunctionalExtensions;
using Errors = Icon.Errors;
using Guid = System.Guid;

namespace Icon.Events
{
    public abstract class AssociationRemovedEvent
      : DeletedEvent, IAssociationRemovedEvent
    {
#nullable disable
        public AssociationRemovedEvent() { }
#nullable enable

        public AssociationRemovedEvent(
            Guid aggregateId,
            Guid creatorId
            )
          : base(
              aggregateId: aggregateId,
              creatorId: creatorId
              )
        {
        }
    }
}