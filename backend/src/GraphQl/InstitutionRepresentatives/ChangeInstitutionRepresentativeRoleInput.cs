using System;

namespace Metabase.GraphQl.InstitutionRepresentatives
{
    public record ChangeInstitutionRepresentativeRoleInput(
          Guid InstitutionId,
          Guid UserId,
          Enumerations.InstitutionRepresentativeRole Role
        );
}