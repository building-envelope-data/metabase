using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;

namespace Icon.GraphQl
{
    public abstract class AddOrRemoveComponentManufacturerPayload
      : Payload
    {
        public ValueObjects.Id ComponentId { get; }
        public ValueObjects.Id InstitutionId { get; }

        public AddOrRemoveComponentManufacturerPayload(
            ValueObjects.Id componentId,
            ValueObjects.Id institutionId,
            ValueObjects.Timestamp requestTimestamp
            )
          : base(requestTimestamp)
        {
            ComponentId = componentId;
            InstitutionId = institutionId;
        }

        public Task<Component> GetComponent(
            [DataLoader] ComponentDataLoader componentLoader
            )
        {
            return componentLoader.LoadAsync(
                TimestampHelpers.TimestampId(ComponentId, RequestTimestamp)
                );
        }

        public Task<Institution> GetInstitution(
            [DataLoader] InstitutionDataLoader institutionLoader
            )
        {
            return institutionLoader.LoadAsync(
                TimestampHelpers.TimestampId(InstitutionId, RequestTimestamp)
                );
        }
    }
}