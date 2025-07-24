using System;
using System.Text.Json;

namespace Metabase.GraphQl.Institutions;

public sealed record SetInstitutionExtrasInput(
    Guid InstitutionId,
    JsonElement? Extras
);