using Infrastructure.Events;
using Infrastructure.ValueObjects;
using Guid = System.Guid;

namespace Metabase.Events
{
    public sealed class InstitutionDeleted
      : DeletedEvent
    {
        public static InstitutionDeleted From<TModel>(
            Infrastructure.Commands.DeleteCommand<TModel> command
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