using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Icon.ValueObjects
{
    public class RemoveOneToManyAssociationInput<TAssociationModel>
      : ValueObject
    {
        public static Result<RemoveOneToManyAssociationInput<TAssociationModel>, Errors> From(
            Id associateId,
            Timestamp timestamp
            )
        {
            return
              Result.Ok<RemoveOneToManyAssociationInput<TAssociationModel>, Errors>(
                  new RemoveOneToManyAssociationInput<TAssociationModel>(
                    associateId: associateId,
                    timestamp: timestamp
                    )
                  );
        }

        public Id AssociateId { get; }
        public Timestamp Timestamp { get; }

        private RemoveOneToManyAssociationInput(
            Id associateId,
            Timestamp timestamp
            )
        {
            AssociateId = associateId;
            Timestamp = timestamp;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return AssociateId;
            yield return Timestamp;
        }
    }
}