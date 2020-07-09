using CSharpFunctionalExtensions;
using Infrastructure.Aggregates;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Database.Aggregates
{
    public sealed class CalorimetricDataAggregate
      : DataAggregate, IConvertible<Models.CalorimetricData>
    {
#nullable disable
        public CalorimetricDataAggregate() { }
#nullable enable

        public void Apply(Marten.Events.Event<Events.CalorimetricDataCreated> @event)
        {
            ApplyCreated(@event);
        }

        public void Apply(Marten.Events.Event<Events.CalorimetricDataDeleted> @event)
        {
            ApplyDeleted(@event);
        }

        public Result<Models.CalorimetricData, Errors> ToModel()
        {
            var virginResult = ValidateNonVirgin();
            if (virginResult.IsFailure)
                return Result.Failure<Models.CalorimetricData, Errors>(virginResult.Error);

            var idResult = Infrastructure.ValueObjects.Id.From(Id);
            var componentIdResult = Infrastructure.ValueObjects.Id.From(ComponentId);
            var dataResult = Infrastructure.ValueObjects.CalorimetricDataJson.FromNestedCollections(Data);
            var timestampResult = Infrastructure.ValueObjects.Timestamp.From(Timestamp);

            return
              Errors.Combine(
                  idResult,
                  componentIdResult,
                  dataResult,
                  timestampResult
                  )
              .Bind(_ =>
                  Models.CalorimetricData.From(
                    id: idResult.Value,
                    componentId: componentIdResult.Value,
                    data: dataResult.Value,
                    timestamp: timestampResult.Value
                    )
                  );
        }
    }
}