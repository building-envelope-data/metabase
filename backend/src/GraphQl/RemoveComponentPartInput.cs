using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Icon.GraphQl
{
    public sealed class RemoveComponentPartInput
      : AddOrRemoveComponentPartInput
    {
        public ValueObjects.Timestamp Timestamp { get; }

        public RemoveComponentPartInput(
            ValueObjects.Id assembledComponentId,
            ValueObjects.Id partComponentId,
            ValueObjects.Timestamp timestamp
            )
          : base(
              assembledComponentId: assembledComponentId,
              partComponentId: partComponentId
              )
        {
            Timestamp = timestamp;
        }

        public static Result<ValueObjects.RemoveManyToManyAssociationInput<Models.ComponentPart>, Errors> Validate(
            RemoveComponentPartInput self,
            IReadOnlyList<object> path
            )
        {
            return ValueObjects.RemoveManyToManyAssociationInput<Models.ComponentPart>.From(
                    parentId: self.AssembledComponentId,
                    associateId: self.PartComponentId,
                    timestamp: self.Timestamp
                  );
        }
    }
}