using System.Collections.Generic;
using System.Collections.ObjectModel; // ReadOnlyDictionary
using CSharpFunctionalExtensions;
using DateTime = System.DateTime;
using Errors = Icon.Errors;
using ValueObjects = Icon.ValueObjects;

namespace Icon.Models
{
    public sealed class OpticalData
      : Model
    {
        public ValueObjects.OpticalDataJson Data { get; }

        private OpticalData(
            ValueObjects.Id id,
            ValueObjects.OpticalDataJson data,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
            Data = data;
        }

        public static Result<OpticalData, Errors> From(
            ValueObjects.Id id,
            ValueObjects.OpticalDataJson data,
            ValueObjects.Timestamp timestamp
            )
        {
            return
              Result.Ok<OpticalData, Errors>(
                  new OpticalData(
                    id: id,
                    data: data,
                    timestamp: timestamp
                    )
                  );
        }
    }
}