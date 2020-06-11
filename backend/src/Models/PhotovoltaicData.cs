using System.Collections.Generic;
using System.Collections.ObjectModel; // ReadOnlyDictionary
using CSharpFunctionalExtensions;
using DateTime = System.DateTime;
using Errors = Icon.Errors;
using ValueObjects = Icon.ValueObjects;

namespace Icon.Models
{
    public sealed class PhotovoltaicData
      : Model
    {
        public ValueObjects.PhotovoltaicDataJson Data { get; }

        private PhotovoltaicData(
            ValueObjects.Id id,
            ValueObjects.PhotovoltaicDataJson data,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
            Data = data;
        }

        public static Result<PhotovoltaicData, Errors> From(
            ValueObjects.Id id,
            ValueObjects.PhotovoltaicDataJson data,
            ValueObjects.Timestamp timestamp
            )
        {
            return
              Result.Ok<PhotovoltaicData, Errors>(
                  new PhotovoltaicData(
                    id: id,
                    data: data,
                    timestamp: timestamp
                    )
                  );
        }
    }
}