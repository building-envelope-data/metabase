using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Database.Queries
{
    public sealed class GetDataOfComponentsQuery<TDataModel>
      : IQuery<IEnumerable<Result<IEnumerable<Result<TDataModel, Errors>>, Errors>>>
    {
        public IReadOnlyCollection<TimestampedId> TimestampedIds { get; }

        private GetDataOfComponentsQuery(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
        {
            TimestampedIds = timestampedIds;
        }

        public static Result<GetDataOfComponentsQuery<TDataModel>, Errors> From(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
        {
            return Result.Success<GetDataOfComponentsQuery<TDataModel>, Errors>(
                    new GetDataOfComponentsQuery<TDataModel>(
                        timestampedIds
                        )
                    );
        }
    }
}