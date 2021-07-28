using System;

namespace Metabase.GraphQl.InstitutionMethodDevelopers
{
    public record ConfirmInstitutionMethodDeveloperInput(
          Guid MethodId,
          Guid InstitutionId
        );
}