using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.GraphQl
{
    public sealed class RemoveComponentPartInput
      : AddOrRemoveComponentPartInput
    {
        public Timestamp Timestamp { get; }

        public RemoveComponentPartInput(
            Id assembledComponentId,
            Id partComponentId,
            Timestamp timestamp
            )
          : base(
              assembledComponentId: assembledComponentId,
              partComponentId: partComponentId
              )
        {
            Timestamp = timestamp;
        }

        public static Result<Infrastructure.ValueObjects.RemoveManyToManyAssociationInput<Models.ComponentPart>, Errors> Validate(
            RemoveComponentPartInput self,
            IReadOnlyList<object> path
            )
        {
            return Infrastructure.ValueObjects.RemoveManyToManyAssociationInput<Models.ComponentPart>.From(
                    parentId: self.AssembledComponentId,
                    associateId: self.PartComponentId,
                    timestamp: self.Timestamp
                  );
        }
    }
}