using ValueObjects = Icon.ValueObjects;
using System;
using System.Collections.Generic;
using Models = Icon.Models;
using Icon.Infrastructure.Query;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;
using System.Linq;

namespace Icon.Queries
{
    public sealed class ListComponentVersions
      : IQuery<ILookup<ValueObjects.TimestampedId, Result<Models.ComponentVersion, Errors>>>
    {
        public IReadOnlyCollection<ValueObjects.TimestampedId> TimestampedComponentIds { get; }

        private ListComponentVersions(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedComponentIds
            )
        {
            TimestampedComponentIds = timestampedComponentIds;
        }

        public static Result<ListComponentVersions, Errors> From(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedComponentIds
            )
        {
            return Result.Ok<ListComponentVersions, Errors>(
                new ListComponentVersions(
                  timestampedComponentIds: timestampedComponentIds
                  )
                );
        }
    }
}