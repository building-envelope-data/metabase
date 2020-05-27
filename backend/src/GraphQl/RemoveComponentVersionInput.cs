using System.Collections.Generic;
using System.Linq;
using ValueObjects = Icon.ValueObjects;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;

namespace Icon.GraphQl
{
    public sealed class RemoveComponentVersionInput
      : AddOrRemoveComponentVersionInput
    {
        public ValueObjects.Timestamp Timestamp { get; }

        public RemoveComponentVersionInput(
            ValueObjects.Id baseComponentId,
            ValueObjects.Id versionComponentId,
            ValueObjects.Timestamp timestamp
            )
          : base(
              baseComponentId: baseComponentId,
              versionComponentId: versionComponentId
              )
        {
            Timestamp = timestamp;
        }

        public static Result<ValueObjects.RemoveManyToManyAssociationInput<Models.ComponentVersion>, Errors> Validate(
            RemoveComponentVersionInput self,
            IReadOnlyList<object> path
            )
        {
            return ValueObjects.RemoveManyToManyAssociationInput<Models.ComponentVersion>.From(
                    parentId: self.BaseComponentId,
                    associateId: self.VersionComponentId,
                    timestamp: self.Timestamp
                  );
        }
    }
}