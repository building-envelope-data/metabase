using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using Infrastructure.ValueObjects;

namespace Metabase.GraphQl
{
    public sealed class CreatePersonPayload
      : CreateOrDeletePersonPayload
    {
        public CreatePersonPayload(
            TimestampedId timestampedId
            )
          : base(timestampedId)
        {
        }

        public Task<Person> GetPerson(
            [DataLoader] PersonDataLoader personLoader
            )
        {
            return personLoader.LoadAsync(
                TimestampHelpers.TimestampId(PersonId, RequestTimestamp)
                );
        }
    }
}