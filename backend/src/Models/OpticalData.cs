using System.Collections.Generic;
using System.Collections.ObjectModel; // ReadOnlyDictionary
using CSharpFunctionalExtensions;
using DateTime = System.DateTime;
using Errors = Icon.Errors;
using ValueObjects = Icon.ValueObjects;

namespace Icon.Models
{
    public sealed class OpticalData
      : DataX<ValueObjects.OpticalDataJson>
    {
        private OpticalData(
            ValueObjects.Id id,
            ValueObjects.Id componentId,
            ValueObjects.OpticalDataJson data,
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

        public static Result<OpticalData, Errors> From(
            ValueObjects.Id id,
            ValueObjects.Id componentId,
            ValueObjects.OpticalDataJson data,
            ValueObjects.Timestamp timestamp
            )
        {
            return
              Result.Ok<OpticalData, Errors>(
                  new OpticalData(
                    id: id,
                    componentId: componentId,
                    data: data,
                    timestamp: timestamp
                    )
                  );
        }
    }
}