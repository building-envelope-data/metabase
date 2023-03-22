using System;

namespace Metabase.GraphQl.InstitutionMethodDevelopers
{
    public record RemoveInstitutionMethodDeveloperInput(
          Guid MethodId,
          Guid InstitutionId
        );
}