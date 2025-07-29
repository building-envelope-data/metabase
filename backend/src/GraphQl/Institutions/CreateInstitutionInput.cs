using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Metabase.GraphQl.Institutions;

public sealed record CreateInstitutionInput(
    Guid? InstitutionId,
    string Name,
    string? Abbreviation,
    string Description,
    Uri? WebsiteLocator,
    string? PublicKey,
    JsonElement? Extras,
    IReadOnlyList<Guid> OwnerIds,
    Guid? ManagerId
);