using CSharpFunctionalExtensions;

namespace Icon.Models
{
    public sealed class HygrothermalData
      : DataX<ValueObjects.HygrothermalDataJson>
    {
        private HygrothermalData(
            ValueObjects.Id id,
            ValueObjects.Id componentId,
            ValueObjects.HygrothermalDataJson data,
            ValueObjects.Timestamp timestamp
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
            ValueObjects.Id id,
            ValueObjects.Id componentId,
            ValueObjects.HygrothermalDataJson data,
            ValueObjects.Timestamp timestamp
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