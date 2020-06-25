using CSharpFunctionalExtensions;

namespace Icon.Models
{
    public sealed class PhotovoltaicDataFromDatabase
      : Model
    {
        public ValueObjects.Id DatabaseId { get; }
        public ValueObjects.Id ComponentId { get; }
        public ValueObjects.PhotovoltaicDataJson Data { get; }

        private PhotovoltaicDataFromDatabase(
            ValueObjects.Id id,
            ValueObjects.Id databaseId,
            ValueObjects.Id componentId,
            ValueObjects.PhotovoltaicDataJson data,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
            DatabaseId = databaseId;
            ComponentId = componentId;
            Data = data;
        }

        public static Result<PhotovoltaicDataFromDatabase, Errors> From(
            ValueObjects.Id id,
            ValueObjects.Id databaseId,
            ValueObjects.Id componentId,
            ValueObjects.PhotovoltaicDataJson data,
            ValueObjects.Timestamp timestamp
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