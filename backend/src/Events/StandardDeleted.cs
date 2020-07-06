using Icon.Infrastructure.Events;
using Guid = System.Guid;

namespace Icon.Events
{
    public sealed class StandardDeleted
      : DeletedEvent
    {
        public static StandardDeleted From<TModel>(
            Commands.Delete<TModel> command
            )
        {
            return new StandardDeleted(
                standardId: command.TimestampedId.Id,
                creatorId: command.CreatorId
                );
        }

#nullable disable
        public StandardDeleted() { }
#nullable enable

        public StandardDeleted(
            Guid standardId,
            Guid creatorId
            )
          : base(
              aggregateId: standardId,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}