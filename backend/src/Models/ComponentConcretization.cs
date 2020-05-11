using ValueObjects = Icon.ValueObjects;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;

namespace Icon.Models
{
    public sealed class ComponentConcretization
      : Model
    {
        public ValueObjects.Id GeneralComponentId { get; }
        public ValueObjects.Id ConcreteComponentId { get; }

        private ComponentConcretization(
            ValueObjects.Id id,
            ValueObjects.Id generalComponentId,
            ValueObjects.Id concreteComponentId,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
            GeneralComponentId = generalComponentId;
            ConcreteComponentId = concreteComponentId;
        }

        public static Result<ComponentConcretization, Errors> From(
            ValueObjects.Id id,
            ValueObjects.Id generalComponentId,
            ValueObjects.Id concreteComponentId,
            ValueObjects.Timestamp timestamp
            )
        {
            return
              Result.Ok<ComponentConcretization, Errors>(
                  new ComponentConcretization(
            id: id,
            generalComponentId: generalComponentId,
            concreteComponentId: concreteComponentId,
            timestamp: timestamp
            )
                  );
        }
    }
}