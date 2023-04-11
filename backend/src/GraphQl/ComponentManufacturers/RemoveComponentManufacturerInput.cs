using System;

namespace Metabase.GraphQl.ComponentManufacturers
{
    public record RemoveComponentManufacturerInput(
          Guid ComponentId,
          Guid InstitutionId
        );
}