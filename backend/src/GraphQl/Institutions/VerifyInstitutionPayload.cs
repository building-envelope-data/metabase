using System.Collections.Generic;

namespace Metabase.GraphQl.Institutions
{
    public sealed class VerifyInstitutionPayload
    {
        public Data.Institution? Institution { get; }
        public IReadOnlyCollection<VerifyInstitutionError>? Errors { get; }

        public VerifyInstitutionPayload(
            Data.Institution institution
            )
        {
            Institution = institution;
        }

        public VerifyInstitutionPayload(
            IReadOnlyCollection<VerifyInstitutionError> errors
            )
        {
            Errors = errors;
        }

        public VerifyInstitutionPayload(
            VerifyInstitutionError error
            )
            : this(new[] { error })
        {
        }
    }
}