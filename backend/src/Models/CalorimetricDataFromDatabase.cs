using CSharpFunctionalExtensions;

namespace Icon.Models
{
    public sealed class CalorimetricDataFromDatabase
      : Model
    {
        public ValueObjects.Id DatabaseId { get; }
        public ValueObjects.Id ComponentId { get; }
        public ValueObjects.CalorimetricDataJson Data { get; }

        private CalorimetricDataFromDatabase(
            ValueObjects.Id id,
            ValueObjects.Id databaseId,
            ValueObjects.Id componentId,
            ValueObjects.CalorimetricDataJson data,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
            DatabaseId = databaseId;
            ComponentId = componentId;
            Data = data;
        }

        public static Result<CalorimetricDataFromDatabase, Errors> From(
            ValueObjects.Id id,
            ValueObjects.Id databaseId,
            ValueObjects.Id componentId,
            ValueObjects.CalorimetricDataJson data,
            ValueObjects.Timestamp timestamp
            )
        {
            return
              Result.Ok<CalorimetricDataFromDatabase, Errors>(
                  new CalorimetricDataFromDatabase(
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