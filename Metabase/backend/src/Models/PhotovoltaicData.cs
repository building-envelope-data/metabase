using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Models
{
    public sealed class PhotovoltaicData
      : DataX<ValueObjects.PhotovoltaicDataJson>
    {
        private PhotovoltaicData(
            Id id,
            Id componentId,
            ValueObjects.PhotovoltaicDataJson data,
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

        public static Result<PhotovoltaicData, Errors> From(
            Id id,
            Id componentId,
            ValueObjects.PhotovoltaicDataJson data,
            Timestamp timestamp
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