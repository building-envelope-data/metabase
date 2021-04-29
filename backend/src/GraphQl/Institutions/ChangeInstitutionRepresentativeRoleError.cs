using System.Collections.Generic;

namespace Metabase.GraphQl.Institutions
{
    public sealed class ChangeInstitutionRepresentativeRoleError
      : UserErrorBase<ChangeInstitutionRepresentativeRoleErrorCode>
    {
        public ChangeInstitutionRepresentativeRoleError(
            ChangeInstitutionRepresentativeRoleErrorCode code,
            string message,
            IReadOnlyList<string> path
            )
          : base(code, message, path)
        {
        }
    }
}