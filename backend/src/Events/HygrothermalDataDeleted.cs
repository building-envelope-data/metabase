using Guid = System.Guid;

namespace Icon.Events
{
    public sealed class HygrothermalDataDeleted
      : DataDeletedEvent
    {
        public static HygrothermalDataDeleted From<TModel>(
            Commands.Delete<TModel> command
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