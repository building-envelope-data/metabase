using System;

namespace Metabase.GraphQl.Institutions
{
    public record ChangeInstitutionRepresentativeRoleInput(
          Guid InstitutionId,
          Guid UserId,
          Enumerations.InstitutionRepresentativeRole NewRole
        );
}