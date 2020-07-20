using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Database.ValueObjects
{
    public sealed class CreateCalorimetricDataInput
      : ValueObject
    {
        public Id ComponentId { get; }
        public CalorimetricDataJson Data { get; }

        private CreateCalorimetricDataInput(
          Id componentId,
          CalorimetricDataJson data
          )
        {
            ComponentId = componentId;
            Data = data;
        }

        public static Result<CreateCalorimetricDataInput, Errors> From(
            Id componentId,
            CalorimetricDataJson data,
            IReadOnlyList<object>? path = null
            )
        {
            return Result.Success<CreateCalorimetricDataInput, Errors>(
                new CreateCalorimetricDataInput(
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