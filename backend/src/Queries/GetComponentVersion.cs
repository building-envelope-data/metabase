using Icon.Infrastructure.Query;
using ValueObjects = Icon.ValueObjects;
using DateTime = System.DateTime;
using Models = Icon.Models;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;

namespace Icon.Queries
{
    public sealed class GetComponentVersion
      : IQuery<Result<Models.ComponentVersion, Errors>>
    {
        public ValueObjects.TimestampedId TimestampedComponentVersionId { get; }

        private GetComponentVersion(
            ValueObjects.TimestampedId timestampedComponentVersionId
            )
        {
            TimestampedComponentVersionId = timestampedComponentVersionId;
        }

        public static Result<GetComponentVersion, Errors> From(
            ValueObjects.TimestampedId timestampedComponentVersionId
            )
        {
            return Result.Ok<GetComponentVersion, Errors>(
                new GetComponentVersion(
                  timestampedComponentVersionId
                  )
                );
        }
    }
}