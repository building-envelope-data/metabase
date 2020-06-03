using CSharpFunctionalExtensions;
using Newtonsoft.Json;
using Commands = Icon.Commands;
using Errors = Icon.Errors;
using Guid = System.Guid;

namespace Icon.Events
{
    public sealed class InstitutionRepresentativeRemoved
      : AssociationRemovedEvent
    {
        public static InstitutionRepresentativeRemoved From(
            Guid institutionRepresentativeId,
            Commands.RemoveAssociation<ValueObjects.RemoveManyToManyAssociationInput<Models.InstitutionRepresentative>> command
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