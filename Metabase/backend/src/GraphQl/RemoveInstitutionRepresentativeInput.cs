using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.GraphQl
{
    public sealed class RemoveInstitutionRepresentativeInput
      : AddOrRemoveInstitutionRepresentativeInput
    {
        public Timestamp Timestamp { get; }

        public RemoveInstitutionRepresentativeInput(
            Id institutionId,
            Id userId,
            Timestamp timestamp
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