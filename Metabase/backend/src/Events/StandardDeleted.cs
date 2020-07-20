using Infrastructure.Events;
using Infrastructure.ValueObjects;
using Guid = System.Guid;

namespace Metabase.Events
{
    public sealed class StandardDeleted
      : DeletedEvent
    {
        public static StandardDeleted From<TModel>(
            Infrastructure.Commands.DeleteCommand<TModel> command
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