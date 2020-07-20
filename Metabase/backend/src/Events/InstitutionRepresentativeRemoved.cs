using Infrastructure.Events;
using Guid = System.Guid;

namespace Metabase.Events
{
    public sealed class InstitutionRepresentativeRemoved
      : AssociationRemovedEvent
    {
        public static InstitutionRepresentativeRemoved From(
            Guid institutionRepresentativeId,
            Infrastructure.Commands.RemoveAssociationCommand<Infrastructure.ValueObjects.RemoveManyToManyAssociationInput<Models.InstitutionRepresentative>> command
            )
        {
            return new InstitutionRepresentativeRemoved(
                institutionRepresentativeId: institutionRepresentativeId,
                creatorId: command.CreatorId
                );
        }

#nullable disable
        public InstitutionRepresentativeRemoved() { }
#nullable enable

        public InstitutionRepresentativeRemoved(
            Guid institutionRepresentativeId,
            Guid creatorId
            )
          : base(
              aggregateId: institutionRepresentativeId,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}