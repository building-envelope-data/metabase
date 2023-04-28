using System;

namespace Metabase.GraphQl.ComponentManufacturers
{
    public sealed record RemoveComponentManufacturerInput(
          Guid ComponentId,
          Guid InstitutionId
        );
}