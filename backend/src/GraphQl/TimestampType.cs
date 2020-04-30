using ValueObjects = Icon.ValueObjects;
using DateTimeType = HotChocolate.Types.DateTimeType;
using DateTimeOffset = System.DateTimeOffset;
using CSharpFunctionalExtensions;

namespace Icon.GraphQl
{
    // This type wraps `DateTimeType` which uses `DateTimeOffset`
    public sealed class TimestampType
        : WrappingScalarType<ValueObjects.Timestamp, DateTimeOffset>
    {
        public TimestampType()
          : base(
              "TimestampType",
              new DateTimeType(),
              dateTimeOffset => ValueObjects.Timestamp.From(dateTimeOffset.DateTime), // TODO Shall we use DateTimeOffset to represent timestamps? They are not fully time-zone aware though as explained on https://docs.microsoft.com/en-us/dotnet/api/system.datetimeoffset?view=netcore-3.1
              timestamp => timestamp.Value
              )
        {
        }
    }
}