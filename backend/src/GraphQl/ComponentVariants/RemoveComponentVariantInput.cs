using System;

namespace Metabase.GraphQl.ComponentVariants
{
    public record RemoveComponentVariantInput(
          Guid OneComponentId,
          Guid OtherComponentId
        );
}