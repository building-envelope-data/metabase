using System.Collections.Generic;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public sealed class AddComponentManufacturerPayload
      : AddOrRemoveComponentManufacturerPayload
    {
        public ComponentManufacturerEdge ComponentManufacturerEdge { get; }
        public ManufacturedComponentEdge ManufacturedComponentEdge { get; }

        public AddComponentManufacturerPayload(
            ComponentManufacturer componentManufacturer
            )
          : base(
              componentId: componentManufacturer.ComponentId,
              institutionId: componentManufacturer.InstitutionId,
              requestTimestamp: componentManufacturer.RequestTimestamp
              )
        {
            ComponentManufacturerEdge = new ComponentManufacturerEdge(componentManufacturer);
            ManufacturedComponentEdge = new ManufacturedComponentEdge(componentManufacturer);
        }
    }
}