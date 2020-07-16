using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Infrastructure.ValueObjects
{
    public sealed class RemoveManyToManyAssociationInput<TAssociationModel>
      : ValueObject
    {
        public static Result<RemoveManyToManyAssociationInput<TAssociationModel>, Errors> From(
            Id parentId,
            Id associateId,
            Timestamp timestamp
            )
        {
            return
              Result.Success<RemoveManyToManyAssociationInput<TAssociationModel>, Errors>(
                  new RemoveManyToManyAssociationInput<TAssociationModel>(
                    parentId: parentId,
                    associateId: associateId,
                    timestamp: timestamp
                    )
                  );
        }

        public Id ParentId { get; }
        public Id AssociateId { get; }
        public Timestamp Timestamp { get; }

        private RemoveManyToManyAssociationInput(
            Id parentId,
            Id associateId,
            Timestamp timestamp
            )
        {
            ParentId = parentId;
            AssociateId = associateId;
            Timestamp = timestamp;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return ParentId;
            yield return AssociateId;
            yield return Timestamp;
        }
    }
}