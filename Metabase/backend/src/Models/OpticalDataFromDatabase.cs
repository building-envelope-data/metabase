using CSharpFunctionalExtensions;
using Infrastructure.Models;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Models
{
    public sealed class OpticalDataFromDatabase
      : Model
    {
        public Id DatabaseId { get; }
        public Id ComponentId { get; }
        public OpticalDataJson Data { get; }

        private OpticalDataFromDatabase(
            Id id,
            Id databaseId,
            Id componentId,
            OpticalDataJson data,
            Timestamp timestamp
            )
          : base(id, timestamp)
        {
            DatabaseId = databaseId;
            ComponentId = componentId;
            Data = data;
        }

        public static Result<OpticalDataFromDatabase, Errors> From(
            Id id,
            Id databaseId,
            Id componentId,
            OpticalDataJson data,
            Timestamp timestamp
            )
        {
            return
              Result.Ok<OpticalDataFromDatabase, Errors>(
                  new OpticalDataFromDatabase(
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