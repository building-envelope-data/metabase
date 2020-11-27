using DateTimeOffset = System.DateTimeOffset;

namespace Infrastructure.GraphQl
{
    // This type wraps `PreciseDateTimeType` which uses `DateTimeOffset`
    public sealed class TimestampType
        : WrappingScalarType<ValueObjects.Timestamp, DateTimeOffset>
    {
        public TimestampType()
          : base(
              "Timestamp",
              new PreciseDateTimeType(),
              dateTimeOffset => ValueObjects.Timestamp.From(dateTimeOffset.UtcDateTime),
              timestamp => timestamp.Value
              )
        {
        }
    }
}