using ValueObjects = Icon.ValueObjects;
using DateTimeType = HotChocolate.Types.DateTimeType;
using DateTime = System.DateTime;
using CSharpFunctionalExtensions;

namespace Icon.GraphQl
{
    public sealed class TimestampType
        : WrappingScalarType<ValueObjects.Timestamp, DateTime>
    {
        public TimestampType()
          : base(
              "TimestampType",
              new DateTimeType(),
              dateTime => ValueObjects.Timestamp.From(dateTime),
              timestamp => timestamp.Value
              )
        {
        }
    }
}