using Guid = System.Guid;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;

namespace Icon.Events
{
    public abstract class RemovedEvent
      : DeletedEvent, IRemovedEvent
    {
        public Guid ParentId { get; set; }
        public Guid AssociateId { get; set; }

#nullable disable
        public RemovedEvent() { }
#nullable enable

        public RemovedEvent(
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