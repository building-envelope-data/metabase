using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Guid = System.Guid;
using Commands = Icon.Commands;

namespace Icon.Events
{
    public sealed class PersonAffiliationAdded
      : CreatedEvent
    {
        public static PersonAffiliationAdded From(
            Guid personAffiliationId,
            Commands.AddPersonAffiliation command
            )
        {
            return new PersonAffiliationAdded(
                personAffiliationId: personAffiliationId,
                personId: command.Input.PersonId,
                institutionId: command.Input.InstitutionId,
                creatorId: command.CreatorId
                );
        }

        public Guid PersonId { get; set; }
        public Guid InstitutionId { get; set; }

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
              creatorId: creatorId
              )
        {
            PersonId = personId;
            InstitutionId = institutionId;
            EnsureValid();
        }

        public override Result<bool, Errors> Validate()
        {
            return
              Result.Combine(
                  base.Validate(),
                  ValidateNonEmpty(PersonId, nameof(PersonId)),
                  ValidateNonEmpty(InstitutionId, nameof(InstitutionId))
                  );
        }
    }
}