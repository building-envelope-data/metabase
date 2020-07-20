using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Database.Queries
{
    public sealed class HasDataForComponentsQuery<TDataModel>
      : IQuery<IEnumerable<Result<bool, Errors>>>
    {
        public IReadOnlyCollection<TimestampedId> TimestampedIds { get; }

        private HasDataForComponentsQuery(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
        {
            TimestampedIds = timestampedIds;
        }

        public static Result<HasDataForComponentsQuery<TDataModel>, Errors> From(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
        {
            return Result.Success<HasDataForComponentsQuery<TDataModel>, Errors>(
                new HasDataForComponentsQuery<TDataModel>(
                  timestampedIds: timestampedIds
                  )
                );
        }
    }
}