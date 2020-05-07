using System.Collections.Generic;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using DateTime = System.DateTime;

namespace Icon.ValueObjects
{
    public sealed class AddComponentPartInput
      : ValueObject
    {
        public Id AssembledComponentId { get; }
        public Id PartComponentId { get; }

        private AddComponentPartInput(
            Id assembledComponentId,
            Id partComponentId
            )
        {
            AssembledComponentId = assembledComponentId;
            PartComponentId = partComponentId;
        }

        public static Result<AddComponentPartInput, Errors> From(
            Id assembledComponentId,
            Id partComponentId
            )
        {
            return
              Result.Ok<AddComponentPartInput, Errors>(
                  new AddComponentPartInput(
                    assembledComponentId: assembledComponentId,
                    partComponentId: partComponentId
                    )
                  );
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return AssembledComponentId;
            yield return PartComponentId;
        }
    }
}