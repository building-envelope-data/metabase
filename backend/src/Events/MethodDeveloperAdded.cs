using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Guid = System.Guid;
using Commands = Icon.Commands;

namespace Icon.Events
{
    public abstract class MethodDeveloperAdded
      : CreatedEvent
    {
        public Guid MethodId { get; set; }
        public Guid StakeholderId { get; set; }

#nullable disable
        public MethodDeveloperAdded() { }
#nullable enable

        protected MethodDeveloperAdded(
            Guid methodDeveloperId,
            Guid methodId,
            Guid stakeholderId,
            Guid creatorId
            )
          : base(
              aggregateId: methodDeveloperId,
              creatorId: creatorId
              )
        {
            MethodId = methodId;
            StakeholderId = stakeholderId;
            EnsureValid();
        }

        public override Result<bool, Errors> Validate()
        {
            return
              Result.Combine(
                  base.Validate(),
                  ValidateNonEmpty(MethodId, nameof(MethodId)),
                  ValidateNonEmpty(StakeholderId, nameof(StakeholderId))
                  );
        }
    }
}