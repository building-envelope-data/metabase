using System.Collections.Generic;

namespace Metabase.GraphQl.Institutions
{
    public sealed class DeleteInstitutionError
      : UserErrorBase<DeleteInstitutionErrorCode>
    {
        public DeleteInstitutionError(
            DeleteInstitutionErrorCode code,
            string message,
            IReadOnlyList<string> path
            )
          : base(code, message, path)
        {
        }
    }
}