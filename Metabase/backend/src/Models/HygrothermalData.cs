using CSharpFunctionalExtensions;
using Infrastructure.Models;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Models
{
    public sealed class HygrothermalData
      : Model
    {
        public Id DatabaseId { get; }
        public Id ComponentId { get; }
        public HygrothermalDataJson Data { get; }

        private HygrothermalData(
            Id id,
            Id databaseId,
            Id componentId,
            HygrothermalDataJson data,
            Timestamp timestamp
            )
          : base(id, timestamp)
        {
            DatabaseId = databaseId;
            ComponentId = componentId;
            Data = data;
        }

        public static Result<HygrothermalData, Errors> From(
            Id id,
            Id databaseId,
            Id componentId,
            HygrothermalDataJson data,
            Timestamp timestamp
            )
        {
            return
              Result.Ok<HygrothermalData, Errors>(
                  new HygrothermalData(
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