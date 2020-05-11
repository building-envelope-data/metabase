using System.Collections.Generic;
using System.Linq;
using ValueObjects = Icon.ValueObjects;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;

namespace Icon.GraphQl
{
    public sealed class AddComponentPartInput
    {
        public ValueObjects.Id AssembledComponentId { get; }
        public ValueObjects.Id PartComponentId { get; }

        private AddComponentPartInput(
            ValueObjects.Id assembledComponentId,
            ValueObjects.Id partComponentId
            )
        {
            AssembledComponentId = assembledComponentId;
            PartComponentId = partComponentId;
        }

        public static Result<ValueObjects.AddComponentPartInput, Errors> Validate(
            AddComponentPartInput self,
            IReadOnlyList<object> path
            )
        {
            return ValueObjects.AddComponentPartInput.From(
                    assembledComponentId: self.AssembledComponentId,
                    partComponentId: self.PartComponentId
                  );
        }
    }
}