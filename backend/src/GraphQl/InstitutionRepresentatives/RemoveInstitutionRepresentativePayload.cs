using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Metabase.GraphQl.Users;
using Metabase.GraphQl.Institutions;

namespace Metabase.GraphQl.InstitutionRepresentatives
{
    public sealed class RemoveInstitutionRepresentativePayload
    {
        private readonly Data.InstitutionRepresentative? _association;
        public IReadOnlyCollection<RemoveInstitutionRepresentativeError>? Errors { get; }

        public RemoveInstitutionRepresentativePayload(
            Data.InstitutionRepresentative institutionRepresentative
        )
        {
            _association = institutionRepresentative;
        }

        public RemoveInstitutionRepresentativePayload(
            IReadOnlyCollection<RemoveInstitutionRepresentativeError> errors
        )
        {
            Errors = errors;
        }

        public RemoveInstitutionRepresentativePayload(
            RemoveInstitutionRepresentativeError error
        )
            : this(new[] { error })
        {
        }

        public async Task<Data.Institution?> GetInstitution(
            InstitutionByIdDataLoader byId,
            CancellationToken cancellationToken
        )
        {
            if (_association is null)
            {
                return null;
            }

            return await byId.LoadAsync(_association.InstitutionId, cancellationToken)!;
        }

        public async Task<Data.User?> GetUser(
            UserByIdDataLoader byId,
            CancellationToken cancellationToken
        )
        {
            if (_association is null)
            {
                return null;
            }

            return await byId.LoadAsync(_association.UserId, cancellationToken)!;
        }
    }
}