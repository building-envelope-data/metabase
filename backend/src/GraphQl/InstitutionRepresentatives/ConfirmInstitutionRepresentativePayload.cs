using System.Collections.Generic;
using Metabase.GraphQl.Users;
using Metabase.GraphQl.Institutions;

namespace Metabase.GraphQl.InstitutionRepresentatives
{
    public sealed class ConfirmInstitutionRepresentativePayload
    {
        public UserRepresentedInstitutionEdge? RepresentedInstitutionEdge { get; }
        public InstitutionRepresentativeEdge? InstitutionRepresentativeEdge { get; }
        public IReadOnlyCollection<ConfirmInstitutionRepresentativeError>? Errors { get; }

        public ConfirmInstitutionRepresentativePayload(
            Data.InstitutionRepresentative institutionRepresentative
        )
        {
            RepresentedInstitutionEdge = new UserRepresentedInstitutionEdge(institutionRepresentative);
            InstitutionRepresentativeEdge = new InstitutionRepresentativeEdge(institutionRepresentative);
        }

        public ConfirmInstitutionRepresentativePayload(
            IReadOnlyCollection<ConfirmInstitutionRepresentativeError> errors
        )
        {
            Errors = errors;
        }

        public ConfirmInstitutionRepresentativePayload(
            ConfirmInstitutionRepresentativeError error
        )
            : this(new[] { error })
        {
        }
    }
}