using Infrastructure.Events;
using Infrastructure.ValueObjects;
using Guid = System.Guid;

namespace Metabase.Events
{
    public sealed class ComponentDeleted
      : DeletedEvent
    {
        public static ComponentDeleted From<TModel>(
            Commands.Delete<TModel> command
            )
        {
            return new ComponentDeleted(
                componentId: command.TimestampedId.Id,
                creatorId: command.CreatorId
                );
        }

#nullable disable
        public ComponentDeleted() { }
#nullable enable

        public ComponentDeleted(
            Guid componentId,
            Guid creatorId
            )
          : base(
              aggregateId: componentId,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}