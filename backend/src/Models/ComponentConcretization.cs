using CSharpFunctionalExtensions;
using Icon.Infrastructure.Models;

namespace Icon.Models
{
    public sealed class ComponentConcretization
      : Model, IManyToManyAssociation
    {
        public ValueObjects.Id GeneralComponentId { get; }
        public ValueObjects.Id ConcreteComponentId { get; }

        public ValueObjects.Id ParentId { get => GeneralComponentId; }
        public ValueObjects.Id AssociateId { get => ConcreteComponentId; }

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