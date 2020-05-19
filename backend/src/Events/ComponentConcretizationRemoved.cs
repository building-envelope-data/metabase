using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Guid = System.Guid;
using Commands = Icon.Commands;
using Newtonsoft.Json;

namespace Icon.Events
{
    public sealed class ComponentConcretizationRemoved
      : RemovedEvent
    {
        public static ComponentConcretizationRemoved From(
            Guid componentConcretizationId,
            Commands.Remove<ValueObjects.RemoveManyToManyAssociationInput<Models.ComponentConcretization>> command
            )
        {
            return new ComponentConcretizationRemoved(
                componentConcretizationId: componentConcretizationId,
                generalComponentId: command.Input.ParentId,
                concreteComponentId: command.Input.AssociateId,
                creatorId: command.CreatorId
                );
        }

        [JsonIgnore]
        public Guid GeneralComponentId { get => ParentId; }

        [JsonIgnore]
        public Guid ConcreteComponentId { get => AssociateId; }

#nullable disable
        public ComponentConcretizationRemoved() { }
#nullable enable

        public ComponentConcretizationRemoved(
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