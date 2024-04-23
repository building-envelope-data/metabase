using System;

namespace Metabase.GraphQl.ComponentVariants;

public sealed record RemoveComponentVariantInput(
    Guid OneComponentId,
    Guid OtherComponentId
);