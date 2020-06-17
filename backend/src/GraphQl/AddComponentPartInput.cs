using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using Errors = Icon.Errors;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public sealed class AddComponentPartInput
      : AddOrRemoveComponentPartInput
    {
        public AddComponentPartInput(
            ValueObjects.Id assembledComponentId,
            ValueObjects.Id partComponentId
            )
          : base(
              assembledComponentId: assembledComponentId,
              partComponentId: partComponentId
              )
        {
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