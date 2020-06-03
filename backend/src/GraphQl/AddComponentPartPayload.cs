using System.Collections.Generic;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
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