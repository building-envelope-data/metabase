using System;

namespace Metabase.GraphQl.InstitutionRepresentatives
{
    public sealed record ChangeInstitutionRepresentativeRoleInput(
        Guid InstitutionId,
        Guid UserId,
        Enumerations.InstitutionRepresentativeRole Role
    );
}