using System;

namespace Metabase.GraphQl.Institutions
{
    public sealed record UpdateInstitutionInput(
          Guid InstitutionId,
          string Name,
          string? Abbreviation,
          string Description,
          Uri? WebsiteLocator,
          string? PublicKey
        );
}