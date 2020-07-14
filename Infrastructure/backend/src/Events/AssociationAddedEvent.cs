using CSharpFunctionalExtensions;
using Errors = Infrastructure.Errors;
using Guid = System.Guid;

namespace Infrastructure.Events
{
    public abstract class AssociationAddedEvent
      : CreatedEvent, IAssociationAddedEvent
    {
        public Guid ParentId { get; set; }
        public Guid AssociateId { get; set; }

#nullable disable
        protected AssociationAddedEvent() { }
#nullable enable

        protected AssociationAddedEvent(
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