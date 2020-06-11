using CSharpFunctionalExtensions;
using Errors = Icon.Errors;
using ValueObjects = Icon.ValueObjects;

namespace Icon.Models
{
    public sealed class ComponentCalorimetricData
      : Model, IOneToManyAssociation
    {
        public ValueObjects.Id ComponentId { get; }
        public ValueObjects.Id CalorimetricDataId { get; }

        public ValueObjects.Id ParentId { get => ComponentId; }
        public ValueObjects.Id AssociateId { get => CalorimetricDataId; }

        private ComponentCalorimetricData(
            ValueObjects.Id id,
            ValueObjects.Id componentId,
            ValueObjects.Id opticalDataId,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
            ComponentId = componentId;
            CalorimetricDataId = opticalDataId;
        }

        public static Result<ComponentCalorimetricData, Errors> From(
            ValueObjects.Id id,
            ValueObjects.Id componentId,
            ValueObjects.Id opticalDataId,
            ValueObjects.Timestamp timestamp
            )
        {
            return
              Result.Ok<ComponentCalorimetricData, Errors>(
                  new ComponentCalorimetricData(
            id: id,
            componentId: componentId,
            opticalDataId: opticalDataId,
            timestamp: timestamp
            )
                  );
        }
    }
}