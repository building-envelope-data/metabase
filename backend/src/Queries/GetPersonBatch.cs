using CSharpFunctionalExtensions;
using Icon.Infrastructure.Query;
using Models = Icon.Models;
using System.Collections.Generic;
using ValueObjects = Icon.ValueObjects;

namespace Icon.Queries
{
    public sealed class GetPersonBatch
      : IQuery<IEnumerable<Result<Models.Person, Errors>>>
    {
        public IReadOnlyCollection<ValueObjects.TimestampedId> TimestampedPersonIds { get; }

        private GetPersonBatch(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedPersonIds
            )
        {
            TimestampedPersonIds = timestampedPersonIds;
        }

        public static Result<GetPersonBatch, Errors> From(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedPersonIds
            )
        {
            return Result.Ok<GetPersonBatch, Errors>(
                    new GetPersonBatch(
                        timestampedPersonIds
                        )
                    );
        }
    }
}