using System;

namespace Metabase.GraphQl.InstitutionRepresentatives
{
    public sealed record ConfirmInstitutionRepresentativeInput(
          Guid InstitutionId,
          Guid UserId
        );
}