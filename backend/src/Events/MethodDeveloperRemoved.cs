using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Guid = System.Guid;
using Commands = Icon.Commands;
using Newtonsoft.Json;

namespace Icon.Events
{
    public abstract class MethodDeveloperRemoved
      : RemovedEvent
    {
        [JsonIgnore]
        public Guid MethodId { get => ParentId; }

        [JsonIgnore]
        public Guid StakeholderId { get => AssociateId; }

#nullable disable
        public MethodDeveloperRemoved() { }
#nullable enable

        protected MethodDeveloperRemoved(
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