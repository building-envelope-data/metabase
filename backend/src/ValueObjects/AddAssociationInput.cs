using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Errors = Icon.Errors;

namespace Icon.ValueObjects
{
    public abstract class AddAssociationInput
      : ValueObject
    {
        public Id ParentId { get; }
        public Id AssociateId { get; }

        protected AddAssociationInput(
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