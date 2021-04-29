using System;

namespace Metabase.GraphQl.Institutions
{
    public record UpdateInstitutionInput(
          Guid InstitutionId,
          string Name,
          string? Abbreviation,
          string Description,
          Uri? WebsiteLocator,
          string? PublicKey,
          Enumerations.InstitutionState State
        );
}