using Validatable = Icon.Validatable;
using Guid = System.Guid;
using Icon.Infrastructure.Query;
/* using ZonedDateTime = NodaTime.ZonedDateTime; */
using DateTime = System.DateTime;
using Models = Icon.Models;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;

namespace Icon.Queries
{
    public class GetComponent
      : Validatable, IQuery<Result<Models.Component, IError>>
    {
        public Guid ComponentId { get; }
        public DateTime Timestamp { get; } // TODO ZonedDateTime

        public GetComponent(
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