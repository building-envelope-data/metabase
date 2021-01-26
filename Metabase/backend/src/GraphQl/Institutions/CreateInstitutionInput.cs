using System.Collections.Generic;
using DateTime = System.DateTime;
using NpgsqlTypes;

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
