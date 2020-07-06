using CSharpFunctionalExtensions;
using Infrastructure.Models;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Models
{
    public sealed class CalorimetricDataFromDatabase
      : Model
    {
        public Id DatabaseId { get; }
        public Id ComponentId { get; }
        public ValueObjects.CalorimetricDataJson Data { get; }

        private CalorimetricDataFromDatabase(
            Id id,
            Id databaseId,
            Id componentId,
            ValueObjects.CalorimetricDataJson data,
            Timestamp timestamp
            )
          : base(id, timestamp)
        {
            DatabaseId = databaseId;
            ComponentId = componentId;
            Data = data;
        }

        public static Result<CalorimetricDataFromDatabase, Errors> From(
            Id id,
            Id databaseId,
            Id componentId,
            ValueObjects.CalorimetricDataJson data,
            Timestamp timestamp
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