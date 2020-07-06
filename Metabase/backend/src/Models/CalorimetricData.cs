using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Models
{
    public sealed class CalorimetricData
      : DataX<ValueObjects.CalorimetricDataJson>
    {
        private CalorimetricData(
            Id id,
            Id componentId,
            ValueObjects.CalorimetricDataJson data,
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
            ValueObjects.CalorimetricDataJson data,
            Timestamp timestamp
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