using ValueObjects = Icon.ValueObjects;
using Icon.Infrastructure.Query;
using DateTime = System.DateTime;
using Models = Icon.Models;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;

namespace Icon.Queries
{
    public sealed class GetComponent
      : IQuery<Result<Models.Component, IError>>
    {
        public ValueObjects.Id ComponentId { get; }
        public ValueObjects.Timestamp Timestamp { get; } // TODO ZonedDateTime

        private GetComponent(
            ValueObjects.Id componentId,
            ValueObjects.Timestamp timestamp
            )
        {
            ComponentId = componentId;
            Timestamp = timestamp;
        }

        public static Result<GetComponent, IError> From(
            ValueObjects.Id componentId,
            ValueObjects.Timestamp timestamp
            )
        {
					return Result.Ok(
							new GetComponent(
                componentId: componentId,
                timestamp: timestamp
								)
							);
        }
    }
}