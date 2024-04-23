using System.Collections.Generic;

namespace Metabase.GraphQl.Institutions
{
    public sealed class VerifyInstitutionError
        : UserErrorBase<VerifyInstitutionErrorCode>
    {
        public VerifyInstitutionError(
            VerifyInstitutionErrorCode code,
            string message,
            IReadOnlyList<string> path
        )
            : base(code, message, path)
        {
        }
    }
}