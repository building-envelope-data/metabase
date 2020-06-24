using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Icon.Infrastructure.Query;
using Models = Icon.Models;
using ValueObjects = Icon.ValueObjects;

namespace Icon.Queries
{
    public sealed class GetDataOfComponents<TDataModel>
      : IQuery<IEnumerable<Result<IEnumerable<Result<TDataModel, Errors>>, Errors>>>
    {
        public IReadOnlyCollection<ValueObjects.TimestampedId> TimestampedIds { get; }

        private GetDataOfComponents(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
        {
            TimestampedIds = timestampedIds;
        }

        public static Result<GetDataOfComponents<TDataModel>, Errors> From(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
        {
            return Result.Ok<GetDataOfComponents<TDataModel>, Errors>(
                    new GetDataOfComponents<TDataModel>(
                        timestampedIds
                        )
                    );
        }
    }
}