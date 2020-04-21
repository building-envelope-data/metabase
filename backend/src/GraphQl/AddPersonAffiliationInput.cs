using System.Collections.Generic;
using System.Linq;
using Guid = System.Guid;
using ValueObjects = Icon.ValueObjects;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;

namespace Icon.GraphQl
{
    public sealed class AddPersonAffiliationInput
    {
        public Guid PersonId { get; }
        public Guid InstitutionId { get; }

        private AddPersonAffiliationInput(
            Guid personId,
            Guid institutionId
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
            var personIdResult = ValueObjects.Id.From(
                self.PersonId,
                path.Append("personId").ToList().AsReadOnly()
                );
            var institutionIdResult = ValueObjects.Id.From(
                self.InstitutionId,
                path.Append("institutionId").ToList().AsReadOnly()
                );

            return
              Errors.Combine(
                  personIdResult,
                  institutionIdResult
                  )
              .Bind(_ =>
                  ValueObjects.AddPersonAffiliationInput.From(
                    personId: personIdResult.Value,
                    institutionId: institutionIdResult.Value
                    )
                  );
        }
    }
}