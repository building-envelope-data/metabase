using System.Collections.Generic;

namespace Metabase.GraphQl.InstitutionRepresentatives
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