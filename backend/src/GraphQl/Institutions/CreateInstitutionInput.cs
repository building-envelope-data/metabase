using System;
using System.Collections.Generic;
using System.Text.Json;
using Metabase.GraphQl.ContactInformations;

namespace Metabase.GraphQl.Institutions;

public sealed record CreateInstitutionInput(
    Guid? InstitutionId,
    string Name,
    string? Abbreviation,
    string Description,
    ContactInformationInput? Contact,
    JsonElement? Extras,
    IReadOnlyList<Guid> OwnerIds,
    Guid? ManagerId
);