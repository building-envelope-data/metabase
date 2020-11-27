using Guid = System.Guid;

namespace Infrastructure.Events
{
    // The difference between the words `remove` and `delete` is explained on
    // https://english.stackexchange.com/questions/52508/difference-between-delete-and-remove
    public interface IDeletedEvent
      : IEvent
    {
        public Guid AggregateId { get; }
    }
}