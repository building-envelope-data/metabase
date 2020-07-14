using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Database.Models
{
    public sealed class PhotovoltaicData
      : DataX<Infrastructure.ValueObjects.PhotovoltaicDataJson>
    {
        private PhotovoltaicData(
            Id id,
            Id componentId,
            Infrastructure.ValueObjects.PhotovoltaicDataJson data,
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
            Infrastructure.ValueObjects.PhotovoltaicDataJson data,
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