using System.Collections.Generic;
using System.Collections.ObjectModel; // ReadOnlyDictionary
using CSharpFunctionalExtensions;
using DateTime = System.DateTime;
using Errors = Icon.Errors;
using ValueObjects = Icon.ValueObjects;

namespace Icon.Models
{
    public sealed class CalorimetricData
      : Model
    {
        public ValueObjects.CalorimetricDataJson Data { get; }

        private CalorimetricData(
            ValueObjects.Id id,
            ValueObjects.CalorimetricDataJson data,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
            Data = data;
        }

        public static Result<CalorimetricData, Errors> From(
            ValueObjects.Id id,
            ValueObjects.CalorimetricDataJson data,
            ValueObjects.Timestamp timestamp
            )
        {
            return
              Result.Ok<CalorimetricData, Errors>(
                  new CalorimetricData(
                    id: id,
                    data: data,
                    timestamp: timestamp
                    )
                  );
        }
    }
}