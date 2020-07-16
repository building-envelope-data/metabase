using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Database.Models
{
    public sealed class CalorimetricData
      : DataX<Infrastructure.ValueObjects.CalorimetricDataJson>
    {
        private CalorimetricData(
            Id id,
            Id componentId,
            Infrastructure.ValueObjects.CalorimetricDataJson data,
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

        public static Result<CalorimetricData, Errors> From(
            Id id,
            Id componentId,
            Infrastructure.ValueObjects.CalorimetricDataJson data,
            Timestamp timestamp
            )
        {
            return
              Result.Success<CalorimetricData, Errors>(
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