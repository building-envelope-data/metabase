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
    public sealed class WhoHasOpticalDataForComponents
      : IQuery<IEnumerable<Result<IEnumerable<Result<Models.Database, Errors>>, Errors>>>
    {
        public IReadOnlyCollection<ValueObjects.TimestampedId> TimestampedIds { get; }

        private WhoHasOpticalDataForComponents(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
        {
            TimestampedIds = timestampedIds;
        }

        public static Result<WhoHasOpticalDataForComponents, Errors> From(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
        {
            return Result.Ok<WhoHasOpticalDataForComponents, Errors>(
                new WhoHasOpticalDataForComponents(
                  timestampedIds: timestampedIds
                  )
                );
        }
    }
}