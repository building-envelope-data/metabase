using System;

namespace Metabase.GraphQl.InstitutionRepresentatives;

public sealed record ForbidPermissionToSignDataInput(
    Guid UserId,
    Guid InstitutionId
);