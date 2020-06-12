using System.Collections.Generic;
using System.Collections.ObjectModel; // ReadOnlyDictionary
using CSharpFunctionalExtensions;
using DateTime = System.DateTime;
using Errors = Icon.Errors;
using ValueObjects = Icon.ValueObjects;

namespace Icon.Models
{
    public sealed class PhotovoltaicData
      : DataX<ValueObjects.PhotovoltaicDataJson>
    {
        private PhotovoltaicData(
            ValueObjects.Id id,
            ValueObjects.Id componentId,
            ValueObjects.PhotovoltaicDataJson data,
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

        public static Result<PhotovoltaicData, Errors> From(
            ValueObjects.Id id,
            ValueObjects.Id componentId,
            ValueObjects.PhotovoltaicDataJson data,
            ValueObjects.Timestamp timestamp
            )
        {
            return
              Result.Ok<PhotovoltaicData, Errors>(
                  new PhotovoltaicData(
                    id: id,
                    componentId: componentId,
                    data: data,
                    timestamp: timestamp
                    )
                  );
        }
    }
}