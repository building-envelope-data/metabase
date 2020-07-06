using Infrastructure.Events;
using Infrastructure.ValueObjects;
using Guid = System.Guid;

namespace Metabase.Events
{
    public sealed class UserDeleted
      : DeletedEvent
    {
        public static UserDeleted From<TModel>(
            Commands.Delete<TModel> command
            )
        {
            return new UserDeleted(
                userId: command.TimestampedId.Id,
                creatorId: command.CreatorId
                );
        }

#nullable disable
        public UserDeleted() { }
#nullable enable

        public UserDeleted(
            Guid userId,
            Guid creatorId
            )
          : base(
              aggregateId: userId,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}