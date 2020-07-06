using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public sealed class AddPersonAffiliationPayload
      : AddOrRemovePersonAffiliationPayload
    {
        public AddPersonAffiliationPayload(
            PersonAffiliation personAffiliation
            )
          : base(
              personId: personAffiliation.PersonId,
              institutionId: personAffiliation.InstitutionId,
              requestTimestamp: personAffiliation.RequestTimestamp
              )
        {
        }
    }
}