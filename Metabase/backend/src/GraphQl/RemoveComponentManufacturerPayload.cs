using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public sealed class RemoveComponentManufacturerPayload
      : AddOrRemoveComponentManufacturerPayload
    {
        public RemoveComponentManufacturerPayload(
            Id componentId,
            Id institutionId,
            Timestamp requestTimestamp
            )
          : base(
              componentId: componentId,
              institutionId: institutionId,
              requestTimestamp: requestTimestamp
              )
        {
        }
    }
}