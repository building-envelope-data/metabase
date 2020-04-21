using CSharpFunctionalExtensions;
using Icon.Infrastructure.Query;
using Models = Icon.Models;
using System.Collections.Generic;
using ValueObjects = Icon.ValueObjects;

namespace Icon.Queries
{
    public sealed class GetStakeholderBatch
      : IQuery<IEnumerable<Result<Models.Stakeholder, Errors>>>
    {
        public IReadOnlyCollection<ValueObjects.TimestampedId> TimestampedStakeholderIds { get; }

        private GetStakeholderBatch(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedStakeholderIds
            )
        {
            TimestampedStakeholderIds = timestampedStakeholderIds;
        }

        public static Result<GetStakeholderBatch, Errors> From(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedStakeholderIds
            )
        {
            return Result.Ok<GetStakeholderBatch, Errors>(
                    new GetStakeholderBatch(
                        timestampedStakeholderIds
                        )
                    );
        }
    }
}