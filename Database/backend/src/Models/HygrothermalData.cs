using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Database.Models
{
    public sealed class HygrothermalData
      : DataX<Infrastructure.ValueObjects.HygrothermalDataJson>
    {
        private HygrothermalData(
            Id id,
            Id componentId,
            Infrastructure.ValueObjects.HygrothermalDataJson data,
            Timestamp timestamp
            )
          : base(
              id: id,
              componentId: componentId,
              data: data,
              timestamp: timestamp
              )
        {
        }

        public static Result<HygrothermalData, Errors> From(
            Id id,
            Id componentId,
            Infrastructure.ValueObjects.HygrothermalDataJson data,
            Timestamp timestamp
            )
        {
            return
              Result.Ok<HygrothermalData, Errors>(
                  new HygrothermalData(
                    id: id,
                    componentId: componentId,
                    data: data,
                    timestamp: timestamp
                    )
                  );
        }
    }
}