using System;

namespace Metabase.GraphQl.ComponentVariants
{
    public sealed record AddComponentVariantInput(
        Guid OneComponentId,
        Guid OtherComponentId
    );
}