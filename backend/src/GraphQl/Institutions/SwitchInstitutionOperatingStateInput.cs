using System;

namespace Metabase.GraphQl.Institutions;

public sealed record SwitchInstitutionOperatingStateInput(
    Guid InstitutionId
);