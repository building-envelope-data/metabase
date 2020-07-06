using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.GraphQl
{
    public sealed class AddPersonAffiliationInput
      : AddOrRemovePersonAffiliationInput
    {
        public AddPersonAffiliationInput(
            Id personId,
            Id institutionId
            )
          : base(
              personId: personId,
              institutionId: institutionId
              )
        {
        }

        public static Result<ValueObjects.AddPersonAffiliationInput, Errors> Validate(
            AddPersonAffiliationInput self,
            IReadOnlyList<object> path
            )
        {
            return ValueObjects.AddPersonAffiliationInput.From(
                    personId: self.PersonId,
                    institutionId: self.InstitutionId
                  );
        }
    }
}