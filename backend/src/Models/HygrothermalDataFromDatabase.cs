using CSharpFunctionalExtensions;
using Icon.Infrastructure.Models;

namespace Icon.Models
{
    public sealed class HygrothermalDataFromDatabase
      : Model
    {
        public ValueObjects.Id DatabaseId { get; }
        public ValueObjects.Id ComponentId { get; }
        public ValueObjects.HygrothermalDataJson Data { get; }

        private HygrothermalDataFromDatabase(
            ValueObjects.Id id,
            ValueObjects.Id databaseId,
            ValueObjects.Id componentId,
            ValueObjects.HygrothermalDataJson data,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
            DatabaseId = databaseId;
            ComponentId = componentId;
            Data = data;
        }

        public static Result<HygrothermalDataFromDatabase, Errors> From(
            ValueObjects.Id id,
            ValueObjects.Id databaseId,
            ValueObjects.Id componentId,
            ValueObjects.HygrothermalDataJson data,
            ValueObjects.Timestamp timestamp
            )
        {
            return
              Result.Ok<HygrothermalDataFromDatabase, Errors>(
                  new HygrothermalDataFromDatabase(
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