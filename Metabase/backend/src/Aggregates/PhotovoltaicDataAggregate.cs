using CSharpFunctionalExtensions;
using Infrastructure.Aggregates;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Aggregates
{
    public sealed class PhotovoltaicDataAggregate
      : DataAggregate, IConvertible<Models.PhotovoltaicData>
    {
#nullable disable
        public PhotovoltaicDataAggregate() { }
#nullable enable

        public void Apply(Marten.Events.Event<Events.PhotovoltaicDataCreated> @event)
        {
            ApplyCreated(@event);
        }

        public void Apply(Marten.Events.Event<Events.PhotovoltaicDataDeleted> @event)
        {
            ApplyDeleted(@event);
        }

        public Result<Models.PhotovoltaicData, Errors> ToModel()
        {
            var virginResult = ValidateNonVirgin();
            if (virginResult.IsFailure)
                return Result.Failure<Models.PhotovoltaicData, Errors>(virginResult.Error);

            var idResult = Infrastructure.ValueObjects.Id.From(Id);
            var componentIdResult = Infrastructure.ValueObjects.Id.From(ComponentId);
            var dataResult = ValueObjects.PhotovoltaicDataJson.FromNestedCollections(Data);
            var timestampResult = Infrastructure.ValueObjects.Timestamp.From(Timestamp);

            return
              Errors.Combine(
                  idResult,
                  componentIdResult,
                  dataResult,
                  timestampResult
                  )
              .Bind(_ =>
                  Models.PhotovoltaicData.From(
                    id: idResult.Value,
                    componentId: componentIdResult.Value,
                    data: dataResult.Value,
                    timestamp: timestampResult.Value
                    )
                  );
        }
    }
}