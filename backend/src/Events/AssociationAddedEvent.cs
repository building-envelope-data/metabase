using CSharpFunctionalExtensions;
using Guid = System.Guid;

namespace Icon.Events
{
    public abstract class AssociationAddedEvent
      : CreatedEvent, IAssociationAddedEvent
    {
        public Guid ParentId { get; set; }
        public Guid AssociateId { get; set; }

#nullable disable
        public AssociationAddedEvent() { }
#nullable enable

        public AssociationAddedEvent(
            Guid aggregateId,
            Guid parentId,
            Guid associateId,
            Guid creatorId
            )
          : base(
              aggregateId: aggregateId,
              creatorId: creatorId
              )
        {
            ParentId = parentId;
            AssociateId = associateId;
        }

        public override Result<bool, Errors> Validate()
        {
            return Result.Combine(
                base.Validate(),
                ValidateNonEmpty(ParentId, nameof(ParentId)),
                ValidateNonEmpty(AssociateId, nameof(AssociateId))
                );
        }
    }
}