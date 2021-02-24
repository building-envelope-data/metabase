using System;

namespace Metabase.GraphQl.Institutions
{
    public record DeleteInstitutionInput(
          Guid InstitutionId
        );
}