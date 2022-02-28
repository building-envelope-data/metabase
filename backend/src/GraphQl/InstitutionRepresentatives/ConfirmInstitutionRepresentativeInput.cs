using System;

namespace Metabase.GraphQl.InstitutionRepresentatives
{
    public record ConfirmInstitutionRepresentativeInput(
          Guid InstitutionId,
          Guid UserId
        );
}