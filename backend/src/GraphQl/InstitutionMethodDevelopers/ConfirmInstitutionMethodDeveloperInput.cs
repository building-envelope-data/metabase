using System;

namespace Metabase.GraphQl.InstitutionMethodDevelopers
{
    public sealed record ConfirmInstitutionMethodDeveloperInput(
          Guid MethodId,
          Guid InstitutionId
        );
}