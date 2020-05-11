using GreenDonut;
using System.Collections.Generic;
using HotChocolate;
using System.Threading.Tasks;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public sealed class CreatePersonPayload
      : Payload
    {
        public ValueObjects.Id PersonId { get; }

        public CreatePersonPayload(
            ValueObjects.TimestampedId timestampedId
            )
          : base(timestampedId.Timestamp)
        {
            PersonId = timestampedId.Id;
        }

        public Task<Person> GetPerson(
            [DataLoader] PersonForTimestampedIdDataLoader personLoader
            )
        {
            return personLoader.LoadAsync(
                TimestampHelpers.TimestampId(PersonId, RequestTimestamp)
                );
        }
    }
}