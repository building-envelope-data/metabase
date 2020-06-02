using ValueObjects = Icon.ValueObjects;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;

namespace Icon.Models
{
    public sealed class ComponentOpticalData
      : Model, IManyToManyAssociation
    {
        public ValueObjects.Id ComponentId { get; }
        public ValueObjects.Id OpticalDataId { get; }

        public ValueObjects.Id ParentId { get => ComponentId; }
        public ValueObjects.Id AssociateId { get => OpticalDataId; }

        private ComponentOpticalData(
            ValueObjects.Id id,
            ValueObjects.Id componentId,
            ValueObjects.Id opticalDataId,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
            ComponentId = componentId;
            OpticalDataId = opticalDataId;
        }

        public static Result<ComponentOpticalData, Errors> From(
            ValueObjects.Id id,
            ValueObjects.Id componentId,
            ValueObjects.Id opticalDataId,
            ValueObjects.Timestamp timestamp
            )
        {
            return
              Result.Ok<ComponentOpticalData, Errors>(
                  new ComponentOpticalData(
            id: id,
            componentId: componentId,
            opticalDataId: opticalDataId,
            timestamp: timestamp
            )
                  );
        }
    }
}