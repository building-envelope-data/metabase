using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;

namespace Icon.GraphQl
{
    public sealed class CreateMethodPayload
      : CreateOrDeleteMethodPayload
    {
        public CreateMethodPayload(
            ValueObjects.TimestampedId timestampedId
            )
          : base(timestampedId)
        {
        }

        public Task<Method> GetMethod(
            [DataLoader] MethodDataLoader methodLoader
            )
        {
            return methodLoader.LoadAsync(
                TimestampHelpers.TimestampId(MethodId, RequestTimestamp)
                );
        }
    }
}