using Icon.Infrastructure.Query;
using ValueObjects = Icon.ValueObjects;
using DateTime = System.DateTime;
using Models = Icon.Models;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;

namespace Icon.Queries
{
    public sealed class GetComponentVersion
      : IQuery<Result<Models.ComponentVersion, IError>>
    {
        public ValueObjects.Id ComponentVersionId { get; }
        public ValueObjects.Timestamp Timestamp { get; }

        private GetComponentVersion(
            ValueObjects.Id componentVersionId,
            ValueObjects.Timestamp timestamp
            )
        {
            ComponentVersionId = componentVersionId;
            Timestamp = timestamp;
        }

        public static Result<GetComponentVersion, IError> From(
            ValueObjects.Id componentVersionId,
            ValueObjects.Timestamp timestamp
            )
        {
					return Result.Ok(
							new GetComponentVersion(
                componentVersionId: componentVersionId,
                timestamp: timestamp
								)
							);
        }
    }
}