using CSharpFunctionalExtensions;
using Errors = Icon.Errors;
using ValueObjects = Icon.ValueObjects;

namespace Icon.Models
{
    public sealed class ComponentPhotovoltaicData
      : Model, IOneToManyAssociation
    {
        public ValueObjects.Id ComponentId { get; }
        public ValueObjects.Id PhotovoltaicDataId { get; }

        public ValueObjects.Id ParentId { get => ComponentId; }
        public ValueObjects.Id AssociateId { get => PhotovoltaicDataId; }

        private ComponentPhotovoltaicData(
            ValueObjects.Id id,
            ValueObjects.Id componentId,
            ValueObjects.Id opticalDataId,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
            ComponentId = componentId;
            PhotovoltaicDataId = opticalDataId;
        }

        public static Result<ComponentPhotovoltaicData, Errors> From(
            ValueObjects.Id id,
            ValueObjects.Id componentId,
            ValueObjects.Id opticalDataId,
            ValueObjects.Timestamp timestamp
            )
        {
            return
              Result.Ok<ComponentPhotovoltaicData, Errors>(
                  new ComponentPhotovoltaicData(
            id: id,
            componentId: componentId,
            opticalDataId: opticalDataId,
            timestamp: timestamp
            )
                  );
        }
    }
}