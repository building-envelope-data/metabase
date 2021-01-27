using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.Institutions
{
    public record CreateInstitutionInput(
          string Name,
          string? Abbreviation,
          string Description,
          string? WebsiteLocator,
          string? PublicKey,
          Enumerations.InstitutionState State,
          IReadOnlyList<Guid> OwnerIds
        );
}