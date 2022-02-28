using System;

namespace Metabase.GraphQl.InstitutionRepresentatives
{
    public record AddInstitutionRepresentativeInput(
          Guid InstitutionId,
          Guid UserId,
          Enumerations.InstitutionRepresentativeRole Role
        );
}