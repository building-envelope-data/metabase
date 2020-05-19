using GreenDonut;
using System.Collections.Generic;
using HotChocolate;
using System.Threading.Tasks;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public abstract class CreateOrDeleteDatabasePayload
      : Payload
    {
        public ValueObjects.Id DatabaseId { get; }

        public CreateOrDeleteDatabasePayload(
            ValueObjects.TimestampedId timestampedId
            )
          : base(timestampedId.Timestamp)
        {
            DatabaseId = timestampedId.Id;
        }
    }
}