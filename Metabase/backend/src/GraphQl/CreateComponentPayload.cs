using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;

namespace Metabase.GraphQl
{
    public sealed class CreateComponentPayload
      : CreateOrDeleteComponentPayload
    {
        public CreateComponentPayload(
            TimestampedId timestampedId
            )
          : base(timestampedId)
        {
        }

        public Task<Component> GetComponent(
            [DataLoader] ComponentDataLoader componentLoader
            )
        {
            return componentLoader.LoadAsync(
                TimestampHelpers.TimestampId(ComponentId, RequestTimestamp)
                );
        }
    }
}