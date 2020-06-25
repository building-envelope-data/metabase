using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;

namespace Icon.GraphQl
{
    public sealed class CreatePersonPayload
      : CreateOrDeletePersonPayload
    {
        public CreatePersonPayload(
            ValueObjects.TimestampedId timestampedId
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