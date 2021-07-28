using System;

namespace Metabase.GraphQl.InstitutionRepresentatives
{
    public record RemoveInstitutionRepresentativeInput(
          Guid InstitutionId,
          Guid UserId
        );
}