using System.Collections.Generic;

namespace Metabase.GraphQl.Institutions
{
    public sealed class AddInstitutionRepresentativeError
      : UserErrorBase<AddInstitutionRepresentativeErrorCode>
    {
        public AddInstitutionRepresentativeError(
            AddInstitutionRepresentativeErrorCode code,
            string message,
            IReadOnlyList<string> path
            )
          : base(code, message, path)
        {
        }
    }
}