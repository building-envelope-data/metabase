using Infrastructure.Events;
using Infrastructure.ValueObjects;
using Guid = System.Guid;

namespace Metabase.Events
{
    public sealed class MethodDeleted
      : DeletedEvent
    {
        public static MethodDeleted From<TModel>(
            Infrastructure.Commands.Delete<TModel> command
            )
        {
            return new MethodDeleted(
                methodId: command.TimestampedId.Id,
                creatorId: command.CreatorId
                );
        }

#nullable disable
        public MethodDeleted() { }
#nullable enable

        public MethodDeleted(
            Guid methodId,
            Guid creatorId
            )
          : base(
              aggregateId: methodId,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}