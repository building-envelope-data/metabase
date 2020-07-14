using Guid = System.Guid;

namespace Infrastructure.Events
{
    public abstract class AssociationRemovedEvent
      : DeletedEvent, IAssociationRemovedEvent
    {
#nullable disable
        protected AssociationRemovedEvent() { }
#nullable enable

        protected AssociationRemovedEvent(
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