using System.Collections.Generic;
using System.Collections.ObjectModel; // ReadOnlyDictionary
using CSharpFunctionalExtensions;
using DateTime = System.DateTime;
using Errors = Icon.Errors;
using ValueObjects = Icon.ValueObjects;

namespace Icon.Models
{
    public sealed class HygrothermalData
      : Model
    {
        public ValueObjects.HygrothermalDataJson Data { get; }

        private HygrothermalData(
            ValueObjects.Id id,
            ValueObjects.HygrothermalDataJson data,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
            Data = data;
        }

        public static Result<HygrothermalData, Errors> From(
            ValueObjects.Id id,
            ValueObjects.HygrothermalDataJson data,
            ValueObjects.Timestamp timestamp
            )
        {
            return
              Result.Ok<HygrothermalData, Errors>(
                  new HygrothermalData(
                    id: id,
                    data: data,
                    timestamp: timestamp
                    )
                  );
        }
    }
}