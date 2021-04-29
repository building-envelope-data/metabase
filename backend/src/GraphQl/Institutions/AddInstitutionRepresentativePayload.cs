using System.Collections.Generic;
using HotChocolate.Types.Pagination;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.Institutions
{
    public sealed class AddInstitutionRepresentativePayload
    {
        public RepresentedInstitutionEdge? RepresentedInstitutionEdge { get; }
        public InstitutionRepresentativeEdge? InstitutionRepresentativeEdge { get; }
        public IReadOnlyCollection<AddInstitutionRepresentativeError>? Errors { get; }

        public AddInstitutionRepresentativePayload(
            Data.InstitutionRepresentative institutionRepresentative
            )
        {
            RepresentedInstitutionEdge = new RepresentedInstitutionEdge(institutionRepresentative);
            InstitutionRepresentativeEdge = new InstitutionRepresentativeEdge(institutionRepresentative);
        }

        public AddInstitutionRepresentativePayload(
            IReadOnlyCollection<AddInstitutionRepresentativeError> errors
            )
        {
            Errors = errors;
        }

        public AddInstitutionRepresentativePayload(
            AddInstitutionRepresentativeError error
            )
            : this(new[] { error })
        {
        }
    }
}