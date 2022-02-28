using System.Collections.Generic;

namespace Metabase.GraphQl.InstitutionRepresentatives
{
    public sealed class RemoveInstitutionRepresentativeError
      : UserErrorBase<RemoveInstitutionRepresentativeErrorCode>
    {
        public RemoveInstitutionRepresentativeError(
            RemoveInstitutionRepresentativeErrorCode code,
            string message,
            IReadOnlyList<string> path
            )
          : base(code, message, path)
        {
        }
    }
}