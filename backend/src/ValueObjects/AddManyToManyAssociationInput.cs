using System.Collections.Generic;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;

namespace Icon.ValueObjects
{
    public abstract class AddManyToManyAssociationInput
      : ValueObject
    {
        public Id ParentId { get; }
        public Id AssociateId { get; }

        protected AddManyToManyAssociationInput(
            Id parentId,
            Id associateId
            )
        {
            ParentId = parentId;
            AssociateId = associateId;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return ParentId;
            yield return AssociateId;
        }
    }
}