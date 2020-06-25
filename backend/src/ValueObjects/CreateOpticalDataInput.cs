using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Icon.ValueObjects
{
    public sealed class CreateOpticalDataInput
      : ValueObject
    {
        public Id ComponentId { get; }
        public OpticalDataJson Data { get; }

        private CreateOpticalDataInput(
          Id componentId,
          OpticalDataJson data
          )
        {
            ComponentId = componentId;
            Data = data;
        }

        public static Result<CreateOpticalDataInput, Errors> From(
            Id componentId,
            OpticalDataJson data,
            IReadOnlyList<object>? path = null
            )
        {
            return Result.Ok<CreateOpticalDataInput, Errors>(
                new CreateOpticalDataInput(
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