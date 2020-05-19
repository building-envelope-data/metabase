using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Guid = System.Guid;
using Commands = Icon.Commands;
using Newtonsoft.Json;

namespace Icon.Events
{
    public sealed class PersonAffiliationRemoved
      : RemovedEvent
    {
        public static PersonAffiliationRemoved From(
            Guid personAffiliationId,
            Commands.Remove<ValueObjects.RemoveManyToManyAssociationInput<Models.PersonAffiliation>> command
            )
        {
            return new PersonAffiliationRemoved(
                personAffiliationId: personAffiliationId,
                personId: command.Input.ParentId,
                institutionId: command.Input.AssociateId,
                creatorId: command.CreatorId
                );
        }

        [JsonIgnore]
        public Guid PersonId { get => ParentId; }

        [JsonIgnore]
        public Guid InstitutionId { get => AssociateId; }

#nullable disable
        public PersonAffiliationRemoved() { }
#nullable enable

        public PersonAffiliationRemoved(
            Guid personAffiliationId,
            Guid personId,
            Guid institutionId,
            Guid creatorId
            )
          : base(
              aggregateId: personAffiliationId,
              parentId: personId,
              associateId: institutionId,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}