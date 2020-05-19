using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Guid = System.Guid;
using Commands = Icon.Commands;
using Newtonsoft.Json;

namespace Icon.Events
{
    public sealed class InstitutionRepresentativeRemoved
      : RemovedEvent
    {
        public static InstitutionRepresentativeRemoved From(
            Guid institutionRepresentativeId,
            Commands.Remove<ValueObjects.RemoveManyToManyAssociationInput<Models.InstitutionRepresentative>> command
            )
        {
            return new InstitutionRepresentativeRemoved(
                institutionRepresentativeId: institutionRepresentativeId,
                institutionId: command.Input.ParentId,
                userId: command.Input.AssociateId,
                creatorId: command.CreatorId
                );
        }

        [JsonIgnore]
        public Guid InstitutionId { get => ParentId; }

        [JsonIgnore]
        public Guid UserId { get => AssociateId; }

#nullable disable
        public InstitutionRepresentativeRemoved() { }
#nullable enable

        public InstitutionRepresentativeRemoved(
            Guid institutionRepresentativeId,
            Guid institutionId,
            Guid userId,
            Guid creatorId
            )
          : base(
              aggregateId: institutionRepresentativeId,
              parentId: institutionId,
              associateId: userId,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}