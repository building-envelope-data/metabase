using CSharpFunctionalExtensions;
using Infrastructure.Models;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Models
{
    public sealed class OpticalData
      : Model
    {
        public Id DatabaseId { get; }
        public Id ComponentId { get; }
        public OpticalDataJson Data { get; }

        private OpticalData(
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

        public static Result<OpticalData, Errors> From(
            Id id,
            Id databaseId,
            Id componentId,
            OpticalDataJson data,
            Timestamp timestamp
            )
        {
            return
              Result.Success<OpticalData, Errors>(
                  new OpticalData(
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