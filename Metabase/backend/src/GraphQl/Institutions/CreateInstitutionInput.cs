using System.Collections.Generic;
using NpgsqlTypes;
using DateTime = System.DateTime;

namespace Metabase.GraphQl.Institutions
{
    public record CreateInstitutionInput(
          string Name,
          string? Abbreviation,
          string Description,
          string? WebsiteLocator,
          string? PublicKey,
          Enumerations.InstitutionState State
        );
}