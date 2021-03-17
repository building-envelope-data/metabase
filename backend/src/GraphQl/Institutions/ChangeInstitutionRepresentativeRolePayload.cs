using System.Collections.Generic;
using HotChocolate.Types.Pagination;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.Institutions
{
    public sealed class ChangeInstitutionRepresentativeRolePayload
    {
        public RepresentedInstitutionEdge? RepresentedInstitutionEdge { get; }
        public InstitutionRepresentativeEdge? InstitutionRepresentativeEdge { get; }
        public IReadOnlyCollection<ChangeInstitutionRepresentativeRoleError>? Errors { get; }

        public ChangeInstitutionRepresentativeRolePayload(
            Data.InstitutionRepresentative institutionRepresentative
            )
        {
            RepresentedInstitutionEdge = new RepresentedInstitutionEdge(institutionRepresentative);
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