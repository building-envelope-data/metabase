using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using Errors = Icon.Errors;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public sealed class RemovePersonAffiliationInput
      : AddOrRemovePersonAffiliationInput
    {
        public ValueObjects.Timestamp Timestamp { get; }

        public RemovePersonAffiliationInput(
            ValueObjects.Id personId,
            ValueObjects.Id institutionId,
            ValueObjects.Timestamp timestamp
            )
          : base(
              personId: personId,
              institutionId: institutionId
              )
        {
            Timestamp = timestamp;
        }

        public static Result<ValueObjects.RemoveManyToManyAssociationInput<Models.PersonAffiliation>, Errors> Validate(
            RemovePersonAffiliationInput self,
            IReadOnlyList<object> path
            )
        {
            return ValueObjects.RemoveManyToManyAssociationInput<Models.PersonAffiliation>.From(
                    parentId: self.PersonId,
                    associateId: self.InstitutionId,
                    timestamp: self.Timestamp
                  );
        }
    }
}