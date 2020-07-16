using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Database.Queries
{
    public sealed class GetDataOfComponents<TDataModel>
      : IQuery<IEnumerable<Result<IEnumerable<Result<TDataModel, Errors>>, Errors>>>
    {
        public IReadOnlyCollection<TimestampedId> TimestampedIds { get; }

        private GetDataOfComponents(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
        {
            TimestampedIds = timestampedIds;
        }

        public static Result<GetDataOfComponents<TDataModel>, Errors> From(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
        {
            return Result.Success<GetDataOfComponents<TDataModel>, Errors>(
                    new GetDataOfComponents<TDataModel>(
                        timestampedIds
                        )
                    );
        }
    }
}