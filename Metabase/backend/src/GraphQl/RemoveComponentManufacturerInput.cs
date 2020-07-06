using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.GraphQl
{
    public sealed class RemoveComponentManufacturerInput
      : AddOrRemoveComponentManufacturerInput
    {
        public Timestamp Timestamp { get; }

        public RemoveComponentManufacturerInput(
            Id componentId,
            Id institutionId,
            Timestamp timestamp
            )
          : base(
              componentId: componentId,
              institutionId: institutionId
              )
        {
            Timestamp = timestamp;
        }

        public static Result<ValueObjects.RemoveManyToManyAssociationInput<Models.ComponentManufacturer>, Errors> Validate(
            RemoveComponentManufacturerInput self,
            IReadOnlyList<object> path
            )
        {
            return ValueObjects.RemoveManyToManyAssociationInput<Models.ComponentManufacturer>.From(
                parentId: self.ComponentId,
                associateId: self.InstitutionId,
                timestamp: self.Timestamp
                );
        }
    }
}