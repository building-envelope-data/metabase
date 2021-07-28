using System.Collections.Generic;

namespace Metabase.GraphQl.InstitutionMethodDevelopers
{
    public sealed class ConfirmInstitutionMethodDeveloperError
      : UserErrorBase<ConfirmInstitutionMethodDeveloperErrorCode>
    {
        public ConfirmInstitutionMethodDeveloperError(
            ConfirmInstitutionMethodDeveloperErrorCode code,
            string message,
            IReadOnlyList<string> path
            )
          : base(code, message, path)
        {
        }
    }
}