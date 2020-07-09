using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.GraphQl
{
    public sealed class RemovePersonAffiliationInput
      : AddOrRemovePersonAffiliationInput
    {
        public Timestamp Timestamp { get; }

        public RemovePersonAffiliationInput(
            Id personId,
            Id institutionId,
            Timestamp timestamp
            )
          : base(
              personId: personId,
              institutionId: institutionId
              )
        {
            Timestamp = timestamp;
        }

        public static Result<Infrastructure.ValueObjects.RemoveManyToManyAssociationInput<Models.PersonAffiliation>, Errors> Validate(
            RemovePersonAffiliationInput self,
            IReadOnlyList<object> path
            )
        {
            return Infrastructure.ValueObjects.RemoveManyToManyAssociationInput<Models.PersonAffiliation>.From(
                    parentId: self.PersonId,
                    associateId: self.InstitutionId,
                    timestamp: self.Timestamp
                  );
        }
    }
}