using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.Institutions
{
    public record CreateInstitutionInput(
          string Name,
          string? Abbreviation,
          string Description,
          Uri? WebsiteLocator,
          string? PublicKey,
          Enumerations.InstitutionState State,
          IReadOnlyList<Guid> OwnerIds
        );
}