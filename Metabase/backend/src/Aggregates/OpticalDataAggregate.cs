using CSharpFunctionalExtensions;
using Infrastructure.Aggregates;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Aggregates
{
    public sealed class OpticalDataAggregate
      : DataAggregate, IConvertible<Models.OpticalData>
    {
#nullable disable
        public OpticalDataAggregate() { }
#nullable enable

        public void Apply(Marten.Events.Event<Events.OpticalDataCreated> @event)
        {
            ApplyCreated(@event);
        }

        public void Apply(Marten.Events.Event<Events.OpticalDataDeleted> @event)
        {
            ApplyDeleted(@event);
        }

        public Result<Models.OpticalData, Errors> ToModel()
        {
            var virginResult = ValidateNonVirgin();
            if (virginResult.IsFailure)
                return Result.Failure<Models.OpticalData, Errors>(virginResult.Error);

            var idResult = Infrastructure.ValueObjects.Id.From(Id);
            var componentIdResult = Infrastructure.ValueObjects.Id.From(ComponentId);
            var dataResult = ValueObjects.OpticalDataJson.FromNestedCollections(Data);
            var timestampResult = Infrastructure.ValueObjects.Timestamp.From(Timestamp);

            return
              Errors.Combine(
                  idResult,
                  componentIdResult,
                  dataResult,
                  timestampResult
                  )
              .Bind(_ =>
                  Models.OpticalData.From(
                    id: idResult.Value,
                    componentId: componentIdResult.Value,
                    data: dataResult.Value,
                    timestamp: timestampResult.Value
                    )
                  );
        }
    }
}