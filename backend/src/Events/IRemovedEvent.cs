using Guid = System.Guid;

namespace Icon.Events
{
    public interface IRemovedEvent
      : IDeletedEvent
    {
        public Guid ParentId { get; }
        public Guid AssociateId { get; }
    }
}