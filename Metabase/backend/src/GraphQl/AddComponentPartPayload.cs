using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public sealed class AddComponentPartPayload
      : AddOrRemoveComponentPartPayload
    {
        public AddComponentPartPayload(
            ComponentPart componentPart
            )
          : base(
              assembledComponentId: componentPart.AssembledComponentId,
              partComponentId: componentPart.PartComponentId,
              requestTimestamp: componentPart.RequestTimestamp
              )
        {
        }
    }
}