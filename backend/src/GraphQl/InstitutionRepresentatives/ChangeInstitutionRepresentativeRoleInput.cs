using System;
using Metabase.Enumerations;

namespace Metabase.GraphQl.InstitutionRepresentatives;

public sealed record ChangeInstitutionRepresentativeRoleInput(
    Guid InstitutionId,
    Guid UserId,
    InstitutionRepresentativeRole Role
);