using System;

namespace Metabase.GraphQl.ComponentManufacturers
{
    public record AddComponentManufacturerInput(
          Guid ComponentId,
          Guid InstitutionId
        );
}