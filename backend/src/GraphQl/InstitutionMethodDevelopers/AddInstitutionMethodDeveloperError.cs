using System.Collections.Generic;

namespace Metabase.GraphQl.InstitutionMethodDevelopers
{
    public sealed class AddInstitutionMethodDeveloperError
      : UserErrorBase<AddInstitutionMethodDeveloperErrorCode>
    {
        public AddInstitutionMethodDeveloperError(
            AddInstitutionMethodDeveloperErrorCode code,
            string message,
            IReadOnlyList<string> path
            )
          : base(code, message, path)
        {
        }
    }
}