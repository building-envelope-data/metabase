using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using Infrastructure.ValueObjects;

namespace Metabase.GraphQl
{
    public abstract class AddOrRemoveComponentConcretizationPayload
      : Payload
    {
        public Id GeneralComponentId { get; }
        public Id ConcreteComponentId { get; }

        protected AddOrRemoveComponentConcretizationPayload(
            Id generalComponentId,
            Id concreteComponentId,
            Timestamp requestTimestamp
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