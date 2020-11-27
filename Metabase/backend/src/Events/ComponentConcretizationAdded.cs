using Infrastructure.Events;
using Newtonsoft.Json;
using Guid = System.Guid;

namespace Metabase.Events
{
    public sealed class ComponentConcretizationAdded
      : AssociationAddedEvent
    {
        public static ComponentConcretizationAdded From(
            Guid componentConcretizationId,
            Infrastructure.Commands.AddAssociationCommand<ValueObjects.AddComponentConcretizationInput> command
            )
        {
            return new ComponentConcretizationAdded(
                componentConcretizationId: componentConcretizationId,
                generalComponentId: command.Input.GeneralComponentId,
                concreteComponentId: command.Input.ConcreteComponentId,
                creatorId: command.CreatorId
                );
        }

        [JsonIgnore]
        public Guid GeneralComponentId { get => ParentId; }

        [JsonIgnore]
        public Guid ConcreteComponentId { get => AssociateId; }

#nullable disable
        public ComponentConcretizationAdded() { }
#nullable enable

        public ComponentConcretizationAdded(
            Guid componentConcretizationId,
            Guid generalComponentId,
            Guid concreteComponentId,
            Guid creatorId
            )
          : base(
              aggregateId: componentConcretizationId,
              parentId: generalComponentId,
              associateId: concreteComponentId,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}