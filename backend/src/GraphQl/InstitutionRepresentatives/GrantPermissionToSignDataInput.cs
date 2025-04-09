using System;

namespace Metabase.GraphQl.InstitutionRepresentatives;

public sealed record GrantPermissionToSignDataInput(
    Guid UserId,
    Guid InstitutionId
);