using Infrastructure.Events;
using Guid = System.Guid;

namespace Metabase.Events
{
    public sealed class InstitutionOperatedDatabaseRemoved
      : AssociationRemovedEvent
    {
        public static InstitutionOperatedDatabaseRemoved From(
            Guid institutionOperatedDatabaseId,
            Infrastructure.Commands.RemoveAssociationCommand<Infrastructure.ValueObjects.RemoveOneToManyAssociationInput<Models.InstitutionOperatedDatabase>> command
            )
        {
            return new InstitutionOperatedDatabaseRemoved(
                institutionOperatedDatabaseId: institutionOperatedDatabaseId,
                creatorId: command.CreatorId
                );
        }

#nullable disable
        public InstitutionOperatedDatabaseRemoved() { }
#nullable enable

        public InstitutionOperatedDatabaseRemoved(
            Guid institutionOperatedDatabaseId,
            Guid creatorId
            )
          : base(
              aggregateId: institutionOperatedDatabaseId,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}