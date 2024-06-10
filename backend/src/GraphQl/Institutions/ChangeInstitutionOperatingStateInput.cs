using System;

namespace Metabase.GraphQl.Institutions;

public sealed record ChangeInstitutionOperatingStateInput(
    Guid InstitutionId
);