using System;

namespace Metabase.GraphQl.ComponentVariants
{
    public record AddComponentVariantInput(
          Guid OneComponentId,
          Guid OtherComponentId
        );
}