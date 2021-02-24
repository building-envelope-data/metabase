using System;

namespace Metabase.GraphQl.Institutions
{
    public record RemoveInstitutionRepresentativeInput(
          Guid InstitutionId,
          Guid UserId
        );
}