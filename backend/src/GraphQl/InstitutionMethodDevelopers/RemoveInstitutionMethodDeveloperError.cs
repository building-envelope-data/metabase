using System.Collections.Generic;

namespace Metabase.GraphQl.InstitutionMethodDevelopers
{
    public sealed class RemoveInstitutionMethodDeveloperError
      : UserErrorBase<RemoveInstitutionMethodDeveloperErrorCode>
    {
        public RemoveInstitutionMethodDeveloperError(
            RemoveInstitutionMethodDeveloperErrorCode code,
            string message,
            IReadOnlyList<string> path
            )
          : base(code, message, path)
        {
        }
    }
}