using CSharpFunctionalExtensions;
using Infrastructure.Models;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Models
{
    public sealed class PhotovoltaicData
      : Model
    {
        public Id DatabaseId { get; }
        public Id ComponentId { get; }
        public PhotovoltaicDataJson Data { get; }

        private PhotovoltaicData(
            Id id,
            Id databaseId,
            Id componentId,
            PhotovoltaicDataJson data,
            Timestamp timestamp
            )
          : base(id, timestamp)
        {
            DatabaseId = databaseId;
            ComponentId = componentId;
            Data = data;
        }

        public static Result<PhotovoltaicData, Errors> From(
            Id id,
            Id databaseId,
            Id componentId,
            PhotovoltaicDataJson data,
            Timestamp timestamp
            )
        {
            return
              Result.Ok<PhotovoltaicData, Errors>(
                  new PhotovoltaicData(
                    id: id,
                    databaseId: databaseId,
                    componentId: componentId,
                    data: data,
                    timestamp: timestamp
                    )
                  );
        }
    }
}