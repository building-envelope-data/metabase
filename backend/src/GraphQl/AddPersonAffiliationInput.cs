using System.Collections.Generic;
using System.Linq;
using ValueObjects = Icon.ValueObjects;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;

namespace Icon.GraphQl
{
    public sealed class AddPersonAffiliationInput
    {
        public ValueObjects.Id PersonId { get; }
        public ValueObjects.Id InstitutionId { get; }

        private AddPersonAffiliationInput(
            ValueObjects.Id personId,
            ValueObjects.Id institutionId
            )
        {
            PersonId = personId;
            InstitutionId = institutionId;
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