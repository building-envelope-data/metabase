using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Guid = System.Guid;
using Commands = Icon.Commands;
using Newtonsoft.Json;

namespace Icon.Events
{
    public sealed class ComponentConcretizationAdded
      : AddedEvent
    {
        public static ComponentConcretizationAdded From(
            Guid componentConcretizationId,
            Commands.Add<ValueObjects.AddComponentConcretizationInput> command
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
        public Guid GeneralComponentId { get => ParentId; set => ParentId = value; }

        [JsonIgnore]
        public Guid ConcreteComponentId { get => AssociateId; set => AssociateId = value; }

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