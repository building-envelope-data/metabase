using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.Institutions;

public sealed record CreateInstitutionInput(
    string Name,
    string? Abbreviation,
    string Description,
    Uri? WebsiteLocator,
    string? PublicKey,
    IReadOnlyList<Guid> OwnerIds,
    Guid? ManagerId
);