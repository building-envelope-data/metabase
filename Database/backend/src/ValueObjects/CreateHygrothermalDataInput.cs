using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Database.ValueObjects
{
    public sealed class CreateHygrothermalDataInput
      : ValueObject
    {
        public Id ComponentId { get; }
        public HygrothermalDataJson Data { get; }

        private CreateHygrothermalDataInput(
          Id componentId,
          HygrothermalDataJson data
          )
        {
            ComponentId = componentId;
            Data = data;
        }

        public static Result<CreateHygrothermalDataInput, Errors> From(
            Id componentId,
            HygrothermalDataJson data,
            IReadOnlyList<object>? path = null
            )
        {
            return Result.Success<CreateHygrothermalDataInput, Errors>(
                new CreateHygrothermalDataInput(
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