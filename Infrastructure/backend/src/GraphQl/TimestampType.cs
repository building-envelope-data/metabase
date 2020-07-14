using Infrastructure.ValueObjects;
using DateTimeOffset = System.DateTimeOffset;

namespace Infrastructure.GraphQl
{
    // This type wraps `PreciseDateTimeType` which uses `DateTimeOffset`
    public sealed class TimestampType
        : WrappingScalarType<Timestamp, DateTimeOffset>
    {
        public TimestampType()
          : base(
              "Timestamp",
              new PreciseDateTimeType(),
              dateTimeOffset => Infrastructure.ValueObjects.Timestamp.From(dateTimeOffset.DateTime), // TODO Shall we use DateTimeOffset to represent timestamps? They are not fully time-zone aware though as explained on https://docs.microsoft.com/en-us/dotnet/api/system.datetimeoffset?view=netcore-3.1
              timestamp => timestamp.Value
              )
        {
        }
    }
}