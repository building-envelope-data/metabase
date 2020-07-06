using Infrastructure.Events;
using Newtonsoft.Json;
using Guid = System.Guid;

namespace Metabase.Events
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