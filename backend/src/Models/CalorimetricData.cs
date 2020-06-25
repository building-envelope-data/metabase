using CSharpFunctionalExtensions;

namespace Icon.Models
{
    public sealed class CalorimetricData
      : DataX<ValueObjects.CalorimetricDataJson>
    {
        private CalorimetricData(
            ValueObjects.Id id,
            ValueObjects.Id componentId,
            ValueObjects.CalorimetricDataJson data,
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

        public static Result<CalorimetricData, Errors> From(
            ValueObjects.Id id,
            ValueObjects.Id componentId,
            ValueObjects.CalorimetricDataJson data,
            ValueObjects.Timestamp timestamp
            )
        {
            return
              Result.Ok<CalorimetricData, Errors>(
                  new CalorimetricData(
                    id: id,
                    componentId: componentId,
                    data: data,
                    timestamp: timestamp
                    )
                  );
        }
    }
}