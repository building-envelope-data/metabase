using System;

namespace Metabase.GraphQl.ComponentManufacturers
{
    public record ConfirmComponentManufacturerInput(
          Guid ComponentId,
          Guid InstitutionId
        );
}