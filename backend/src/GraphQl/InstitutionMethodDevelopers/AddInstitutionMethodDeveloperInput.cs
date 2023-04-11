using System;

namespace Metabase.GraphQl.InstitutionMethodDevelopers
{
    public record AddInstitutionMethodDeveloperInput(
          Guid MethodId,
          Guid InstitutionId
        );
}