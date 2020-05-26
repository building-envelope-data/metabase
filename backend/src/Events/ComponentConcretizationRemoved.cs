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
                creatorId: command.CreatorId
                );
        }

#nullable disable
        public ComponentConcretizationRemoved() { }
#nullable enable

        public ComponentConcretizationRemoved(
            Guid componentConcretizationId,
            Guid creatorId
            )
          : base(
              aggregateId: componentConcretizationId,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}