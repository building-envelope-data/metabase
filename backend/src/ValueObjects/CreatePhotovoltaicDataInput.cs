using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Icon.ValueObjects
{
    public sealed class CreatePhotovoltaicDataInput
      : ValueObject
    {
        public Id ComponentId { get; }
        public PhotovoltaicDataJson Data { get; }

        private CreatePhotovoltaicDataInput(
          Id componentId,
          PhotovoltaicDataJson data
          )
        {
            ComponentId = componentId;
            Data = data;
        }

        public static Result<CreatePhotovoltaicDataInput, Errors> From(
            Id componentId,
            PhotovoltaicDataJson data,
            IReadOnlyList<object>? path = null
            )
        {
            return Result.Ok<CreatePhotovoltaicDataInput, Errors>(
                new CreatePhotovoltaicDataInput(
                  componentId,
                  data
                  )
                );
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return ComponentId;
            yield return Data;
        }
    }
}