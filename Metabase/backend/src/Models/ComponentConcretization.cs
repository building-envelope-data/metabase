using CSharpFunctionalExtensions;
using Infrastructure.Models;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Models
{
    public sealed class ComponentConcretization
      : Model, IManyToManyAssociation
    {
        public Id GeneralComponentId { get; }
        public Id ConcreteComponentId { get; }

        public Id ParentId { get => GeneralComponentId; }
        public Id AssociateId { get => ConcreteComponentId; }

        private ComponentConcretization(
            Id id,
            Id generalComponentId,
            Id concreteComponentId,
            Timestamp timestamp
            )
          : base(id, timestamp)
        {
            GeneralComponentId = generalComponentId;
            ConcreteComponentId = concreteComponentId;
        }

        public static Result<ComponentConcretization, Errors> From(
            Id id,
            Id generalComponentId,
            Id concreteComponentId,
            Timestamp timestamp
            )
        {
            return
              Result.Success<ComponentConcretization, Errors>(
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