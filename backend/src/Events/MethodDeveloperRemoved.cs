using Guid = System.Guid;

namespace Icon.Events
{
    public abstract class MethodDeveloperRemoved
      : AssociationRemovedEvent
    {
#nullable disable
        public MethodDeveloperRemoved() { }
#nullable enable

        protected MethodDeveloperRemoved(
            Guid methodDeveloperId,
            Guid creatorId
            )
          : base(
              aggregateId: methodDeveloperId,
              creatorId: creatorId
              )
        {
        }
    }
}