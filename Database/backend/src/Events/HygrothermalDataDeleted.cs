using Infrastructure.ValueObjects;
using Guid = System.Guid;

namespace Database.Events
{
    public sealed class HygrothermalDataDeleted
      : DataDeletedEvent
    {
        public static HygrothermalDataDeleted From<TModel>(
            Infrastructure.Commands.DeleteCommand<TModel> command
            )
        {
            return new HygrothermalDataDeleted(
                hygrothermalDataId: command.TimestampedId.Id,
                creatorId: command.CreatorId
                );
        }

#nullable disable
        public HygrothermalDataDeleted() { }
#nullable enable

        public HygrothermalDataDeleted(
            Guid hygrothermalDataId,
            Guid creatorId
            )
          : base(
              aggregateId: hygrothermalDataId,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}