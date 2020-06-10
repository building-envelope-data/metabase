using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using Icon.Infrastructure.Query;
using IError = HotChocolate.IError;
using Models = Icon.Models;
using ValueObjects = Icon.ValueObjects;

namespace Icon.Queries
{
    public sealed class GetOpticalDataIkdbOfComponents
      : IQuery<IEnumerable<Result<IEnumerable<Result<Models.OpticalDataIkdb, Errors>>, Errors>>>
    {
        public IReadOnlyCollection<ValueObjects.TimestampedId> TimestampedIds { get; }

        private GetOpticalDataIkdbOfComponents(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
        {
            TimestampedIds = timestampedIds;
        }

        public static Result<GetOpticalDataIkdbOfComponents, Errors> From(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
        {
            return Result.Ok<GetOpticalDataIkdbOfComponents, Errors>(
                new GetOpticalDataIkdbOfComponents(
                  timestampedIds: timestampedIds
                  )
                );
        }
    }
}