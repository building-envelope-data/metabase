using GreenDonut;
using System.Collections.Generic;
using HotChocolate;
using System.Threading.Tasks;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public sealed class AddComponentManufacturerPayload
      : Payload
    {
        public ValueObjects.Id ComponentId { get; }
        public ValueObjects.Id InstitutionId { get; }
        public ComponentManufacturerEdge ComponentManufacturerEdge { get; }
        public ManufacturedComponentEdge ManufacturedComponentEdge { get; }

        public AddComponentManufacturerPayload(
            ComponentManufacturer componentManufacturer
            )
          : base(componentManufacturer.RequestTimestamp)
        {
            ComponentId = componentManufacturer.ComponentId;
            InstitutionId = componentManufacturer.InstitutionId;
            ComponentManufacturerEdge = new ComponentManufacturerEdge(componentManufacturer);
            ManufacturedComponentEdge = new ManufacturedComponentEdge(componentManufacturer);
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