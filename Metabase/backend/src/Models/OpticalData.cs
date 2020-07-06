using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Models
{
    public sealed class OpticalData
      : DataX<ValueObjects.OpticalDataJson>
    {
        private OpticalData(
            Id id,
            Id componentId,
            ValueObjects.OpticalDataJson data,
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
            ValueObjects.OpticalDataJson data,
            Timestamp timestamp
            )
        {
            return
              Result.Ok<OpticalData, Errors>(
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