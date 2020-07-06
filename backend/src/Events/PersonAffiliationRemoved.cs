using Icon.Infrastructure.Events;
using Guid = System.Guid;

namespace Icon.Events
{
    public sealed class PersonAffiliationRemoved
      : AssociationRemovedEvent
    {
        public static PersonAffiliationRemoved From(
            Guid personAffiliationId,
            Commands.RemoveAssociation<ValueObjects.RemoveManyToManyAssociationInput<Models.PersonAffiliation>> command
            )
        {
            return new PersonAffiliationRemoved(
                personAffiliationId: personAffiliationId,
                creatorId: command.CreatorId
                );
        }

#nullable disable
        public PersonAffiliationRemoved() { }
#nullable enable

        public PersonAffiliationRemoved(
            Guid personAffiliationId,
            Guid creatorId
            )
          : base(
              aggregateId: personAffiliationId,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}