using System;
using System.Globalization;
using HotChocolate.Types;

// The class `PreciseDateTimeType` is a literal copy of `DateTimeType`, see
// https://github.com/ChilliCream/hotchocolate/blob/10.4.1/src/Core/Types/Types/Scalars/DateTimeType.cs
// With the only exception that an extra `fff` was added to each of the
// constants `_(utc|local)Format` to make the number of `f`s equal to 6 making
// microseconds being included in serializations, which is the precision of
// PostgreSQL (date) times, see
// https://www.postgresql.org/docs/12/datatype-datetime.html#DATATYPE-DATETIME-INPUT
// TODO ask the developers of HotChocolate to make their `DateTimeType`
// serialization include microseconds or at least to make the precision
// configurable.
// TODO Update this class with each upgrade of HotChocolate!
namespace Metabase.GraphQl
{
    public sealed class PreciseDateTimeType
        : DateTimeTypeBase
    {
        private const string _utcFormat = "yyyy-MM-ddTHH\\:mm\\:ss.ffffffZ";
        private const string _localFormat = "yyyy-MM-ddTHH\\:mm\\:ss.ffffffzzz";

        public PreciseDateTimeType()
            : base("DateTime")
        {
            Description = "The `DateTime` scalar represents an ISO-8601 compliant date time type."; // TypeResources.DateTimeType_Description;
        }

        public override Type ClrType => typeof(DateTimeOffset);

        protected override string Serialize(DateTime value)
        {
            if (value.Kind == DateTimeKind.Utc)
            {
                return value.ToString(
                    _utcFormat,
                    CultureInfo.InvariantCulture);
            }

            return value.ToString(
                _localFormat,
                CultureInfo.InvariantCulture);
        }

        protected override string Serialize(DateTimeOffset value)
        {
            if (value.Offset == TimeSpan.Zero)
            {
                return value.ToString(
                    _utcFormat,
                    CultureInfo.InvariantCulture);
            }

            return value.ToString(
                _localFormat,
                CultureInfo.InvariantCulture);
        }

        public override bool TryDeserialize(object? serialized, out object? value)
        {
            if (serialized is null)
            {
                value = null;
                return true;
            }

            if (serialized is string s
                && TryDeserializeFromString(s, out object? d))
            {
                value = d;
                return true;
            }

            if (serialized is DateTimeOffset)
            {
                value = serialized;
                return true;
            }

            if (serialized is DateTime dt)
            {
                value = new DateTimeOffset(
                    dt.ToUniversalTime(),
                    TimeSpan.Zero);
                return true;
            }

            value = null;
            return false;
        }

        protected override bool TryDeserializeFromString(
            string? serialized,
            out object? obj)
        {
            if (serialized != null
                && serialized.EndsWith("Z")
                && DateTime.TryParse(
                    serialized,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal,
                    out DateTime zuluTime))
            {
                obj = new DateTimeOffset(
                    zuluTime.ToUniversalTime(),
                    TimeSpan.Zero);
                return true;
            }

            if (DateTimeOffset.TryParse(
                serialized,
                out DateTimeOffset dateTime))
            {
                obj = dateTime;
                return true;
            }

            obj = null;
            return false;
        }
    }
}