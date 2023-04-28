using System;

namespace Metabase.GraphQl.Institutions
{
    public sealed record DeleteInstitutionInput(
          Guid InstitutionId
        );
}