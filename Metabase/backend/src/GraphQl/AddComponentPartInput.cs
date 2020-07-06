using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.GraphQl
{
    public sealed class AddComponentPartInput
      : AddOrRemoveComponentPartInput
    {
        public AddComponentPartInput(
            Id assembledComponentId,
            Id partComponentId
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