using Infrastructure.Events;
using Newtonsoft.Json;
using Guid = System.Guid;

namespace Metabase.Events
{
    public sealed class PersonAffiliationAdded
      : AssociationAddedEvent
    {
        public static PersonAffiliationAdded From(
            Guid personAffiliationId,
            Infrastructure.Commands.AddAssociation<ValueObjects.AddPersonAffiliationInput> command
            )
        {
            return new PersonAffiliationAdded(
                personAffiliationId: personAffiliationId,
                personId: command.Input.PersonId,
                institutionId: command.Input.InstitutionId,
                creatorId: command.CreatorId
                );
        }

        [JsonIgnore]
        public Guid PersonId { get => ParentId; }

        [JsonIgnore]
        public Guid InstitutionId { get => AssociateId; }

#nullable disable
        public PersonAffiliationAdded() { }
#nullable enable

        public PersonAffiliationAdded(
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