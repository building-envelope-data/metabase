using System;

namespace Metabase.GraphQl.ComponentManufacturers;

public sealed record AddComponentManufacturerInput(
    Guid ComponentId,
    Guid InstitutionId
);