using Validatable = Icon.Validatable;
using System.Collections.Generic;
using Guid = System.Guid;
using Icon.Infrastructure.Query;
/* using ZonedDateTime = NodaTime.ZonedDateTime; */
using DateTime = System.DateTime;
using Models = Icon.Models;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;
using System.Linq;

namespace Icon.Queries
{
    public class GetComponentBatch
      : Validatable, IQuery<IEnumerable<Result<Models.Component, IError>>>
    {
        public IEnumerable<(Guid Id, DateTime Timestamp)> ComponentIdsAndTimestamps { get; }

        public GetComponentBatch(
            IEnumerable<(Guid Id, DateTime Timestamp)> componentIdsAndTimestamps
            )
        {
            ComponentIdsAndTimestamps = componentIdsAndTimestamps;
            EnsureValid();
        }

        public override bool IsValid()
        {
            return
              ComponentIdsAndTimestamps.All(
                  ((Guid Id, DateTime Timestamp) idAndTimestamp)
                  => idAndTimestamp.Id != Guid.Empty &&
                     idAndTimestamp.Timestamp != DateTime.MinValue
                  );
        }
    }
}