using Guid = System.Guid;

namespace Icon.Events
{
    public sealed class MethodDeleted
      : DeletedEvent
    {
        public static MethodDeleted From<TModel>(
            Commands.Delete<TModel> command
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