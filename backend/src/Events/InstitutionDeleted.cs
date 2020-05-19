using Guid = System.Guid;

namespace Icon.Events
{
    public sealed class InstitutionDeleted
      : DeletedEvent
    {
        public static InstitutionDeleted From<TModel>(
            Commands.Delete<TModel> command
            )
        {
            return new InstitutionDeleted(
                institutionId: command.TimestampedId.Id,
                creatorId: command.CreatorId
                );
        }

#nullable disable
        public InstitutionDeleted() { }
#nullable enable

        public InstitutionDeleted(
            Guid institutionId,
            Guid creatorId
            )
          : base(
              aggregateId: institutionId,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}