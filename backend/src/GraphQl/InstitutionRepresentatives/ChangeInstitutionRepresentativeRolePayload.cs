using System.Collections.Generic;
using Metabase.GraphQl.Users;
using Metabase.GraphQl.Institutions;

namespace Metabase.GraphQl.InstitutionRepresentatives
{
    public sealed class ChangeInstitutionRepresentativeRolePayload
    {
        public UserRepresentedInstitutionEdge? RepresentedInstitutionEdge { get; }
        public InstitutionRepresentativeEdge? InstitutionRepresentativeEdge { get; }
        public IReadOnlyCollection<ChangeInstitutionRepresentativeRoleError>? Errors { get; }

        public ChangeInstitutionRepresentativeRolePayload(
            Data.InstitutionRepresentative institutionRepresentative
        )
        {
            RepresentedInstitutionEdge = new UserRepresentedInstitutionEdge(institutionRepresentative);
            InstitutionRepresentativeEdge = new InstitutionRepresentativeEdge(institutionRepresentative);
        }

        public ChangeInstitutionRepresentativeRolePayload(
            IReadOnlyCollection<ChangeInstitutionRepresentativeRoleError> errors
        )
        {
            Errors = errors;
        }

        public ChangeInstitutionRepresentativeRolePayload(
            ChangeInstitutionRepresentativeRoleError error
        )
            : this(new[] { error })
        {
        }
    }
}