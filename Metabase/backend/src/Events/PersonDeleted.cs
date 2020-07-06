using Infrastructure.Events;
using Infrastructure.ValueObjects;
using Guid = System.Guid;

namespace Metabase.Events
{
    public sealed class PersonDeleted
      : DeletedEvent
    {
        public static PersonDeleted From<TModel>(
            Commands.Delete<TModel> command
            )
        {
            return new PersonDeleted(
                personId: command.TimestampedId.Id,
                creatorId: command.CreatorId
                );
        }

#nullable disable
        public PersonDeleted() { }
#nullable enable

        public PersonDeleted(
            Guid personId,
            Guid creatorId
            )
          : base(
              aggregateId: personId,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}