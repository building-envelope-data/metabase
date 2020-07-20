using Infrastructure.Events;
using Infrastructure.ValueObjects;
using Guid = System.Guid;

namespace Metabase.Events
{
    public sealed class DatabaseDeleted
      : DeletedEvent
    {
        public static DatabaseDeleted From<TModel>(
            Infrastructure.Commands.DeleteCommand<TModel> command
            )
        {
            return new DatabaseDeleted(
                databaseId: command.TimestampedId.Id,
                creatorId: command.CreatorId
                );
        }

#nullable disable
        public DatabaseDeleted() { }
#nullable enable

        public DatabaseDeleted(
            Guid databaseId,
            Guid creatorId
            )
          : base(
              aggregateId: databaseId,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}