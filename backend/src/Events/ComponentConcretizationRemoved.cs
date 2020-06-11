using CSharpFunctionalExtensions;
using Newtonsoft.Json;
using Commands = Icon.Commands;
using Errors = Icon.Errors;
using Guid = System.Guid;

namespace Icon.Events
{
    public sealed class ComponentConcretizationRemoved
      : AssociationRemovedEvent
    {
        public static ComponentConcretizationRemoved From(
            Guid componentConcretizationId,
            Commands.RemoveAssociation<ValueObjects.RemoveManyToManyAssociationInput<Models.ComponentConcretization>> command
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