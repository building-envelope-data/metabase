using Validatable = Icon.Validatable;
using System;
using System.Collections.Generic;
using Models = Icon.Models;
using Icon.Infrastructure.Query;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;

namespace Icon.Queries
{
    public class ListComponentVersions
      : Validatable, IQuery<IEnumerable<Result<Models.ComponentVersion, IError>>>
    {
        public Guid ComponentId { get; }
        public DateTime Timestamp { get; }

        public ListComponentVersions(
            Guid componentId,
            DateTime timestamp
            )
        {
            ComponentId = componentId;
            Timestamp = timestamp;
            EnsureValid();
        }

        public override bool IsValid()
        {
            return
              ComponentId != Guid.Empty &&
              Timestamp != DateTime.MinValue;
        }
    }
}