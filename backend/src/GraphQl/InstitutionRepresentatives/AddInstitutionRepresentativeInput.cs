using System;

namespace Metabase.GraphQl.InstitutionRepresentatives
{
    public sealed record AddInstitutionRepresentativeInput(
        Guid InstitutionId,
        Guid UserId,
        Enumerations.InstitutionRepresentativeRole Role
    );
}