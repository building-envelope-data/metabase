using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Database.Models
{
    public sealed class OpticalData
      : DataX<Infrastructure.ValueObjects.OpticalDataJson>
    {
        private OpticalData(
            Id id,
            Id componentId,
            Infrastructure.ValueObjects.OpticalDataJson data,
            Timestamp timestamp
            )
          : base(
              id: id,
              componentId: componentId,
              data: data,
              timestamp: timestamp
              )
        {
        }

        public static Result<OpticalData, Errors> From(
            Id id,
            Id componentId,
            Infrastructure.ValueObjects.OpticalDataJson data,
            Timestamp timestamp
            )
        {
            return
              Result.Success<OpticalData, Errors>(
                  new OpticalData(
                    id: id,
                    componentId: componentId,
                    data: data,
                    timestamp: timestamp
                    )
                  );
        }
    }
}