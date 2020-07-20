using Infrastructure.ValueObjects;
using Guid = System.Guid;

namespace Database.Events
{
    public sealed class OpticalDataDeleted
      : DataDeletedEvent
    {
        public static OpticalDataDeleted From<TModel>(
            Infrastructure.Commands.DeleteCommand<TModel> command
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