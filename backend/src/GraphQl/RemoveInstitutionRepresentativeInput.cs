using System.Collections.Generic;
using System.Linq;
using Errors = Icon.Errors;
using ValueObjects = Icon.ValueObjects;
using CSharpFunctionalExtensions;

namespace Icon.GraphQl
{
    public sealed class RemoveInstitutionRepresentativeInput
      : AddOrRemoveInstitutionRepresentativeInput
    {
        public ValueObjects.Timestamp Timestamp { get; }

        public RemoveInstitutionRepresentativeInput(
            ValueObjects.Id institutionId,
            ValueObjects.Id userId,
            ValueObjects.Timestamp timestamp
            )
          : base(
              institutionId: institutionId,
              userId: userId
              )
        {
            Timestamp = timestamp;
        }

        public static Result<ValueObjects.RemoveManyToManyAssociationInput<Models.InstitutionRepresentative>, Errors> Validate(
            RemoveInstitutionRepresentativeInput self,
            IReadOnlyList<object> path
            )
        {
            return ValueObjects.RemoveManyToManyAssociationInput<Models.InstitutionRepresentative>.From(
                    parentId: self.InstitutionId,
                    associateId: self.UserId,
                    timestamp: self.Timestamp
                    );
        }
    }
}