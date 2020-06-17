using CSharpFunctionalExtensions;
using Newtonsoft.Json;
using Commands = Icon.Commands;
using Errors = Icon.Errors;
using Guid = System.Guid;

namespace Icon.Events
{
    public abstract class MethodDeveloperAdded
      : AssociationAddedEvent
    {
        [JsonIgnore]
        public Guid MethodId { get => ParentId; }

        [JsonIgnore]
        public Guid StakeholderId { get => AssociateId; }

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
              parentId: methodId,
              associateId: stakeholderId,
              creatorId: creatorId
              )
        {
        }
    }
}