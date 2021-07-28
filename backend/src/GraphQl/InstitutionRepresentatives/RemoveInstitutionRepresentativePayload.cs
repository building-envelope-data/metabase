using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using Metabase.GraphQl.Users;
using Metabase.GraphQl.Institutions;

namespace Metabase.GraphQl.InstitutionRepresentatives
{
    public sealed class RemoveInstitutionRepresentativePayload
    {
        private readonly Guid _institutionId;
        private readonly Guid _userId;
        public IReadOnlyCollection<RemoveInstitutionRepresentativeError>? Errors { get; }

        public RemoveInstitutionRepresentativePayload(
            Data.InstitutionRepresentative institutionRepresentative
            )
        {
            _institutionId = institutionRepresentative.InstitutionId;
            _userId = institutionRepresentative.UserId;
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

        public Task<Data.Institution> GetInstitution(
            [DataLoader] InstitutionByIdDataLoader byId,
            CancellationToken cancellationToken
            )
        {
            return byId.LoadAsync(_institutionId, cancellationToken)!;
        }

        public Task<Data.User> GetUser(
            [DataLoader] UserByIdDataLoader byId,
            CancellationToken cancellationToken
            )
        {
            return byId.LoadAsync(_userId, cancellationToken)!;
        }
    }
}