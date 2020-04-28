using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Guid = System.Guid;
using Commands = Icon.Commands;
using Newtonsoft.Json;

namespace Icon.Events
{
    public abstract class MethodDeveloperAdded
      : AddedEvent
    {
        [JsonIgnore]
        public Guid MethodId { get => ParentId; set => ParentId = value; }

        [JsonIgnore]
        public Guid StakeholderId { get => AssociateId; set => AssociateId = value; }

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
            EnsureValid();
        }
    }
}