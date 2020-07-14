using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.GraphQl
{
    public sealed class RemoveComponentVersionInput
      : AddOrRemoveComponentVersionInput
    {
        public Timestamp Timestamp { get; }

        public RemoveComponentVersionInput(
            Id baseComponentId,
            Id versionComponentId,
            Timestamp timestamp
            )
          : base(
              baseComponentId: baseComponentId,
              versionComponentId: versionComponentId
              )
        {
            Timestamp = timestamp;
        }

        public static Result<Infrastructure.ValueObjects.RemoveManyToManyAssociationInput<Models.ComponentVersion>, Errors> Validate(
            RemoveComponentVersionInput self,
            IReadOnlyList<object> path
            )
        {
            return Infrastructure.ValueObjects.RemoveManyToManyAssociationInput<Models.ComponentVersion>.From(
                    parentId: self.BaseComponentId,
                    associateId: self.VersionComponentId,
                    timestamp: self.Timestamp
                  );
        }
    }
}