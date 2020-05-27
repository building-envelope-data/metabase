using GreenDonut;
using System.Collections.Generic;
using HotChocolate;
using System.Threading.Tasks;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
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