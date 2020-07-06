using CSharpFunctionalExtensions;
using Infrastructure.Aggregates;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Aggregates
{
    public sealed class HygrothermalDataAggregate
      : DataAggregate, IConvertible<Models.HygrothermalData>
    {
#nullable disable
        public HygrothermalDataAggregate() { }
#nullable enable

        public void Apply(Marten.Events.Event<Events.HygrothermalDataCreated> @event)
        {
            ApplyCreated(@event);
        }

        public void Apply(Marten.Events.Event<Events.HygrothermalDataDeleted> @event)
        {
            ApplyDeleted(@event);
        }

        public Result<Models.HygrothermalData, Errors> ToModel()
        {
            var virginResult = ValidateNonVirgin();
            if (virginResult.IsFailure)
                return Result.Failure<Models.HygrothermalData, Errors>(virginResult.Error);

            var idResult = Infrastructure.ValueObjects.Id.From(Id);
            var componentIdResult = Infrastructure.ValueObjects.Id.From(ComponentId);
            var dataResult = ValueObjects.HygrothermalDataJson.FromNestedCollections(Data);
            var timestampResult = Infrastructure.ValueObjects.Timestamp.From(Timestamp);

            return
              Errors.Combine(
                  idResult,
                  componentIdResult,
                  dataResult,
                  timestampResult
                  )
              .Bind(_ =>
                  Models.HygrothermalData.From(
                    id: idResult.Value,
                    componentId: componentIdResult.Value,
                    data: dataResult.Value,
                    timestamp: timestampResult.Value
                    )
                  );
        }
    }
}