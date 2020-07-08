using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using Infrastructure.ValueObjects;

namespace Metabase.GraphQl
{
    public abstract class AddOrRemoveComponentManufacturerPayload
      : Payload
    {
        public Id ComponentId { get; }
        public Id InstitutionId { get; }

        protected AddOrRemoveComponentManufacturerPayload(
            Id componentId,
            Id institutionId,
            Timestamp requestTimestamp
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