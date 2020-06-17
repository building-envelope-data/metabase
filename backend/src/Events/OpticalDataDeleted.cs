using Guid = System.Guid;

namespace Icon.Events
{
    public sealed class OpticalDataDeleted
      : DataDeletedEvent
    {
        public static OpticalDataDeleted From<TModel>(
            Commands.Delete<TModel> command
            )
        {
            return new OpticalDataDeleted(
                opticalDataId: command.TimestampedId.Id,
                creatorId: command.CreatorId
                );
        }

#nullable disable
        public OpticalDataDeleted() { }
#nullable enable

        public OpticalDataDeleted(
            Guid opticalDataId,
            Guid creatorId
            )
          : base(
              aggregateId: opticalDataId,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}