using System;
using System.Text.Json;
using Metabase.GraphQl.ContactInformations;

namespace Metabase.GraphQl.Institutions;

public sealed record UpdateInstitutionInput(
    Guid InstitutionId,
    string Name,
    string? Abbreviation,
    string Description,
    ContactInformationInput? Contact,
    JsonElement? Extras
);