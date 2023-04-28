using System;

namespace Metabase.GraphQl.ComponentManufacturers
{
    public sealed record ConfirmComponentManufacturerInput(
          Guid ComponentId,
          Guid InstitutionId
        );
}