using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;

namespace Icon.GraphQl
{
    public abstract class AddOrRemoveComponentConcretizationPayload
      : Payload
    {
        public ValueObjects.Id GeneralComponentId { get; }
        public ValueObjects.Id ConcreteComponentId { get; }

        public AddOrRemoveComponentConcretizationPayload(
            ValueObjects.Id generalComponentId,
            ValueObjects.Id concreteComponentId,
            ValueObjects.Timestamp requestTimestamp
            )
          : base(requestTimestamp)
        {
            GeneralComponentId = generalComponentId;
            ConcreteComponentId = concreteComponentId;
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