using Infrastructure.ValueObjects;
using Guid = System.Guid;

namespace Database.Events
{
    public sealed class CalorimetricDataDeleted
      : DataDeletedEvent
    {
        public static CalorimetricDataDeleted From<TModel>(
            Infrastructure.Commands.Delete<TModel> command
            )
        {
            return new CalorimetricDataDeleted(
                calorimetricDataId: command.TimestampedId.Id,
                creatorId: command.CreatorId
                );
        }

#nullable disable
        public CalorimetricDataDeleted() { }
#nullable enable

        public CalorimetricDataDeleted(
            Guid calorimetricDataId,
            Guid creatorId
            )
          : base(
              aggregateId: calorimetricDataId,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}