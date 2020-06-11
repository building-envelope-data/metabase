using CSharpFunctionalExtensions;
using Errors = Icon.Errors;
using ValueObjects = Icon.ValueObjects;

namespace Icon.Models
{
    public sealed class ComponentHygrothermalData
      : Model, IOneToManyAssociation
    {
        public ValueObjects.Id ComponentId { get; }
        public ValueObjects.Id HygrothermalDataId { get; }

        public ValueObjects.Id ParentId { get => ComponentId; }
        public ValueObjects.Id AssociateId { get => HygrothermalDataId; }

        private ComponentHygrothermalData(
            ValueObjects.Id id,
            ValueObjects.Id componentId,
            ValueObjects.Id opticalDataId,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
            ComponentId = componentId;
            HygrothermalDataId = opticalDataId;
        }

        public static Result<ComponentHygrothermalData, Errors> From(
            ValueObjects.Id id,
            ValueObjects.Id componentId,
            ValueObjects.Id opticalDataId,
            ValueObjects.Timestamp timestamp
            )
        {
            return
              Result.Ok<ComponentHygrothermalData, Errors>(
                  new ComponentHygrothermalData(
            id: id,
            componentId: componentId,
            opticalDataId: opticalDataId,
            timestamp: timestamp
            )
                  );
        }
    }
}