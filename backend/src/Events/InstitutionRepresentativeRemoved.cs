using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Guid = System.Guid;
using Commands = Icon.Commands;
using Newtonsoft.Json;

namespace Icon.Events
{
    public sealed class InstitutionRepresentativeRemoved
      : AssociationRemovedEvent
    {
        public static InstitutionRepresentativeRemoved From(
            Guid institutionRepresentativeId,
            Commands.Remove<ValueObjects.RemoveManyToManyAssociationInput<Models.InstitutionRepresentative>> command
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