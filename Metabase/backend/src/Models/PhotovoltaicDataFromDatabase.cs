using CSharpFunctionalExtensions;
using Infrastructure.Models;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Models
{
    public sealed class PhotovoltaicDataFromDatabase
      : Model
    {
        public Id DatabaseId { get; }
        public Id ComponentId { get; }
        public ValueObjects.PhotovoltaicDataJson Data { get; }

        private PhotovoltaicDataFromDatabase(
            Id id,
            Id databaseId,
            Id componentId,
            ValueObjects.PhotovoltaicDataJson data,
            Timestamp timestamp
            )
          : base(id, timestamp)
        {
            DatabaseId = databaseId;
            ComponentId = componentId;
            Data = data;
        }

        public static Result<PhotovoltaicDataFromDatabase, Errors> From(
            Id id,
            Id databaseId,
            Id componentId,
            ValueObjects.PhotovoltaicDataJson data,
            Timestamp timestamp
            )
        {
            return
              Result.Ok<PhotovoltaicDataFromDatabase, Errors>(
                  new PhotovoltaicDataFromDatabase(
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