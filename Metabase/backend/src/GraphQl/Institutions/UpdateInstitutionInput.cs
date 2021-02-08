using System;

namespace Metabase.GraphQl.Institutions
{
    public record UpdateInstitutionInput(
          Guid InstitutionId,
          string Name,
          string? Abbreviation,
          string Description,
          string? WebsiteLocator,
          string? PublicKey,
          Enumerations.InstitutionState State
        );
}