using GreenDonut;
using System.Collections.Generic;
using HotChocolate;
using System.Threading.Tasks;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public sealed class AddComponentConcretizationPayload
      : Payload
    {
        public ValueObjects.Id GeneralComponentId { get; }
        public ValueObjects.Id ConcreteComponentId { get; }

        public AddComponentConcretizationPayload(
            ComponentConcretization componentConcretization
            )
          : base(componentConcretization.RequestTimestamp)
        {
            GeneralComponentId = componentConcretization.GeneralComponentId;
            ConcreteComponentId = componentConcretization.ConcreteComponentId;
        }

        public Task<Component> GetGeneralComponent(
            [DataLoader] ComponentDataLoader componentLoader
            )
        {
            return componentLoader.LoadAsync(
                TimestampHelpers.TimestampId(GeneralComponentId, RequestTimestamp)
                );
        }

        public Task<Component> GetConcreteComponent(
            [DataLoader] ComponentDataLoader componentLoader
            )
        {
            return componentLoader.LoadAsync(
                TimestampHelpers.TimestampId(ConcreteComponentId, RequestTimestamp)
                );
        }
    }
}