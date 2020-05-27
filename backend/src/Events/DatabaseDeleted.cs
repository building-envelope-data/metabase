using Guid = System.Guid;

namespace Icon.Events
{
    public sealed class DatabaseDeleted
      : DeletedEvent
    {
        public static DatabaseDeleted From<TModel>(
            Commands.Delete<TModel> command
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