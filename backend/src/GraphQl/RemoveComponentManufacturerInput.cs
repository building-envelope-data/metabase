using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using Errors = Icon.Errors;
using Uri = System.Uri;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public sealed class RemoveComponentManufacturerInput
      : AddOrRemoveComponentManufacturerInput
    {
        public ValueObjects.Timestamp Timestamp { get; }

        public RemoveComponentManufacturerInput(
            ValueObjects.Id componentId,
            ValueObjects.Id institutionId,
            ValueObjects.Timestamp timestamp
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