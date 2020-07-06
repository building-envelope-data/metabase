using CSharpFunctionalExtensions;
using Infrastructure.Models;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Models
{
    public sealed class HygrothermalDataFromDatabase
      : Model
    {
        public Id DatabaseId { get; }
        public Id ComponentId { get; }
        public ValueObjects.HygrothermalDataJson Data { get; }

        private HygrothermalDataFromDatabase(
            Id id,
            Id databaseId,
            Id componentId,
            ValueObjects.HygrothermalDataJson data,
            Timestamp timestamp
            )
          : base(id, timestamp)
        {
            DatabaseId = databaseId;
            ComponentId = componentId;
            Data = data;
        }

        public static Result<HygrothermalDataFromDatabase, Errors> From(
            Id id,
            Id databaseId,
            Id componentId,
            ValueObjects.HygrothermalDataJson data,
            Timestamp timestamp
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