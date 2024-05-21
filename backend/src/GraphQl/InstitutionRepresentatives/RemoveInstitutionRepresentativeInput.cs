using System;

namespace Metabase.GraphQl.InstitutionRepresentatives;

public sealed record RemoveInstitutionRepresentativeInput(
    Guid InstitutionId,
    Guid UserId
);