using CSharpFunctionalExtensions;
using Icon.Infrastructure.Query;
using Models = Icon.Models;
using System.Collections.Generic;
using ValueObjects = Icon.ValueObjects;

namespace Icon.Queries
{
    public sealed class GetInstitutionBatch
      : IQuery<IEnumerable<Result<Models.Institution, Errors>>>
    {
        public IReadOnlyCollection<ValueObjects.TimestampedId> TimestampedInstitutionIds { get; }

        private GetInstitutionBatch(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedInstitutionIds
            )
        {
            TimestampedInstitutionIds = timestampedInstitutionIds;
        }

        public static Result<GetInstitutionBatch, Errors> From(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedInstitutionIds
            )
        {
            return Result.Ok<GetInstitutionBatch, Errors>(
                    new GetInstitutionBatch(
                        timestampedInstitutionIds
                        )
                    );
        }
    }
}