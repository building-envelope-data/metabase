using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;

namespace Metabase.GraphQl
{
    public sealed class CreateMethodPayload
      : CreateOrDeleteMethodPayload
    {
        public CreateMethodPayload(
            TimestampedId timestampedId
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