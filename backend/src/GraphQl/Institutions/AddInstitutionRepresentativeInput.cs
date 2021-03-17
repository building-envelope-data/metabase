using System;

namespace Metabase.GraphQl.Institutions
{
    public record AddInstitutionRepresentativeInput(
          Guid InstitutionId,
          Guid UserId,
          Enumerations.InstitutionRepresentativeRole Role
        );
}