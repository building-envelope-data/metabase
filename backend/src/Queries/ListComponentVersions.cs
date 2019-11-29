using ValueObjects = Icon.ValueObjects;
using System;
using System.Collections.Generic;
using Models = Icon.Models;
using Icon.Infrastructure.Query;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;

namespace Icon.Queries
{
    public sealed class ListComponentVersions
      : IQuery<IEnumerable<Result<Models.ComponentVersion, Errors>>>
    {
        public ValueObjects.Id ComponentId { get; }
        public ValueObjects.Timestamp Timestamp { get; }

        private ListComponentVersions(
            ValueObjects.Id componentId,
            ValueObjects.Timestamp timestamp
            )
        {
            ComponentId = componentId;
            Timestamp = timestamp;
        }

        public static Result<ListComponentVersions, Errors> From(
            ValueObjects.Id componentId,
            ValueObjects.Timestamp timestamp
            )
        {
            return Result.Ok<ListComponentVersions, Errors>(
                new ListComponentVersions(
                  componentId: componentId,
                  timestamp: timestamp
                  )
                );
        }
    }
}