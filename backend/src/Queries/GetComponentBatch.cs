using System.Collections.Generic;
using Icon.Infrastructure.Query;
using ValueObjects = Icon.ValueObjects;
using DateTime = System.DateTime;
using Models = Icon.Models;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;
using System.Linq;

namespace Icon.Queries
{
    public sealed class GetComponentBatch
      : IQuery<IEnumerable<Result<Models.Component, IError>>>
    {
        public IReadOnlyCollection<(ValueObjects.Id, ValueObjects.Timestamp)> ComponentIdsAndTimestamps { get; }

        private GetComponentBatch(
            IReadOnlyCollection<(ValueObjects.Id, ValueObjects.Timestamp)> componentIdsAndTimestamps
            )
        {
            ComponentIdsAndTimestamps = componentIdsAndTimestamps;
        }

        public static Result<GetComponentBatch, IError> From(
            IReadOnlyCollection<(ValueObjects.Id, ValueObjects.Timestamp)> componentIdsAndTimestamps
            )
        {
					return Result.Ok(
							new GetComponentBatch(
                componentIdsAndTimestamps: componentIdsAndTimestamps
								)
							);
        }
    }
}