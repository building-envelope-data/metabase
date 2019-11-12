using System.Collections.Generic;
using Guid = System.Guid;
using Icon.Infrastructure.Query;
/* using ZonedDateTime = NodaTime.ZonedDateTime; */
using DateTime = System.DateTime;
using Models = Icon.Models;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;

namespace Icon.Queries
{
    public class GetComponentBatch
      : IQuery<IEnumerable<Result<Models.Component, IError>>>
    {
        public IEnumerable<(Guid Id, DateTime Timestamp)> ComponentIdsAndTimestamps { get; }

        public GetComponentBatch(
            IEnumerable<(Guid Id, DateTime Timestamp)> componentIdsAndTimestamps
            )
        {
            ComponentIdsAndTimestamps = componentIdsAndTimestamps;
        }
    }
}