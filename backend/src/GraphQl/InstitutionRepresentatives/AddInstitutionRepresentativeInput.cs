using System;
using Metabase.Enumerations;

namespace Metabase.GraphQl.InstitutionRepresentatives;

public sealed record AddInstitutionRepresentativeInput(
    Guid InstitutionId,
    Guid UserId,
    InstitutionRepresentativeRole Role
);