using System.Collections.Generic;

namespace Metabase.GraphQl.InstitutionRepresentatives
{
    public sealed class ConfirmInstitutionRepresentativeError
      : UserErrorBase<ConfirmInstitutionRepresentativeErrorCode>
    {
        public ConfirmInstitutionRepresentativeError(
            ConfirmInstitutionRepresentativeErrorCode code,
            string message,
            IReadOnlyList<string> path
            )
          : base(code, message, path)
        {
        }
    }
}