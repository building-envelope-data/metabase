using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Guid = System.Guid;
using Commands = Icon.Commands;

namespace Icon.Events
{
    public sealed class PersonAffiliationAdded : Event
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

        public Guid PersonAffiliationId { get; set; }
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
          : base(creatorId)
        {
            PersonAffiliationId = personAffiliationId;
            PersonId = personId;
            InstitutionId = institutionId;
            EnsureValid();
        }

        public override Result<bool, Errors> Validate()
        {
            return
              Result.Combine(
                  base.Validate(),
                  ValidateNonEmpty(PersonAffiliationId, nameof(PersonAffiliationId)),
                  ValidateNonEmpty(PersonId, nameof(PersonId)),
                  ValidateNonEmpty(InstitutionId, nameof(InstitutionId))
                  );
        }
    }
}