using ValueObjects = Icon.ValueObjects;
using Icon.Infrastructure.Query;
using DateTime = System.DateTime;
using Models = Icon.Models;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;

namespace Icon.Queries
{
    public sealed class GetComponent
      : IQuery<Result<Models.Component, Errors>>
    {
        public ValueObjects.TimestampedId TimestampedComponentId { get; }

        private GetComponent(
            ValueObjects.TimestampedId timestampedComponentId
            )
        {
            TimestampedComponentId = timestampedComponentId;
        }

        public static Result<GetComponent, Errors> From(
            ValueObjects.TimestampedId timestampedComponentId
            )
        {
            return Result.Ok<GetComponent, Errors>(
                    new GetComponent(
                      timestampedComponentId
                        )
                    );
        }
    }
}