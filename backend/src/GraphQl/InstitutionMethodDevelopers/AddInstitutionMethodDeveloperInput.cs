using System;

namespace Metabase.GraphQl.InstitutionMethodDevelopers
{
    public sealed record AddInstitutionMethodDeveloperInput(
          Guid MethodId,
          Guid InstitutionId
        );
}