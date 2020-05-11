using GreenDonut;
using System.Collections.Generic;
using HotChocolate;
using System.Threading.Tasks;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public sealed class CreateMethodPayload
      : Payload
    {
        public ValueObjects.Id MethodId { get; }

        public CreateMethodPayload(
            ValueObjects.TimestampedId timestampedId
            )
          : base(timestampedId.Timestamp)
        {
            MethodId = timestampedId.Id;
        }

        public Task<Method> GetMethod(
            [DataLoader] MethodForTimestampedIdDataLoader methodLoader
            )
        {
            return methodLoader.LoadAsync(
                TimestampHelpers.TimestampId(MethodId, RequestTimestamp)
                );
        }
    }
}