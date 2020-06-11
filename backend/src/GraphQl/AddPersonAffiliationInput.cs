using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using Errors = Icon.Errors;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public sealed class AddPersonAffiliationInput
      : AddOrRemovePersonAffiliationInput
    {
        public AddPersonAffiliationInput(
            ValueObjects.Id personId,
            ValueObjects.Id institutionId
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