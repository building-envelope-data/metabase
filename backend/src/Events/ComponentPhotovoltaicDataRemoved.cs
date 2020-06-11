using CSharpFunctionalExtensions;
using Newtonsoft.Json;
using Commands = Icon.Commands;
using Errors = Icon.Errors;
using Guid = System.Guid;

namespace Icon.Events
{
    public sealed class ComponentPhotovoltaicDataRemoved
      : AssociationRemovedEvent
    {
        public static ComponentPhotovoltaicDataRemoved From(
            Guid componentPhotovoltaicDataId,
            Commands.RemoveAssociation<ValueObjects.RemoveOneToManyAssociationInput<Models.ComponentPhotovoltaicData>> command
            )
        {
            return new ComponentPhotovoltaicDataRemoved(
                componentPhotovoltaicDataId: componentPhotovoltaicDataId,
                creatorId: command.CreatorId
                );
        }

#nullable disable
        public ComponentPhotovoltaicDataRemoved() { }
#nullable enable

        public ComponentPhotovoltaicDataRemoved(
            Guid componentPhotovoltaicDataId,
            Guid creatorId
            )
          : base(
              aggregateId: componentPhotovoltaicDataId,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}