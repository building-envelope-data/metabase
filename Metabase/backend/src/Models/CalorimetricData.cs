using CSharpFunctionalExtensions;
using Infrastructure.Models;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Models
{
    public sealed class CalorimetricData
      : Model
    {
        public Id DatabaseId { get; }
        public Id ComponentId { get; }
        public CalorimetricDataJson Data { get; }

        private CalorimetricData(
            Id id,
            Id databaseId,
            Id componentId,
            CalorimetricDataJson data,
            Timestamp timestamp
            )
          : base(id, timestamp)
        {
            DatabaseId = databaseId;
            ComponentId = componentId;
            Data = data;
        }

        public static Result<CalorimetricData, Errors> From(
            Id id,
            Id databaseId,
            Id componentId,
            CalorimetricDataJson data,
            Timestamp timestamp
            )
        {
            return
              Result.Success<CalorimetricData, Errors>(
                  new CalorimetricData(
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