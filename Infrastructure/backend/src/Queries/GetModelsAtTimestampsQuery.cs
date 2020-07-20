using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Infrastructure.Queries
{
    public sealed class GetModelsAtTimestampsQuery<M>
      : IQuery<IEnumerable<Result<IEnumerable<Result<M, Errors>>, Errors>>>
    {
        public IReadOnlyCollection<Timestamp> Timestamps { get; }

        private GetModelsAtTimestampsQuery(
            IReadOnlyCollection<Timestamp> timestamps
            )
        {
            Timestamps = timestamps;
        }

        public static Result<GetModelsAtTimestampsQuery<M>, Errors> From(
            IReadOnlyCollection<Timestamp> timestamps
            )
        {
            return Result.Success<GetModelsAtTimestampsQuery<M>, Errors>(
                    new GetModelsAtTimestampsQuery<M>(
                        timestamps
                        )
                    );
        }
    }
}