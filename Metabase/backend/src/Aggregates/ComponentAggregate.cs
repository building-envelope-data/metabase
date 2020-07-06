using CSharpFunctionalExtensions;
using Infrastructure.Aggregates;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Aggregates
{
    public sealed class ComponentAggregate
      : EventSourcedAggregate, IConvertible<Models.Component>
    {
        public ComponentInformationAggregateData Information { get; set; }

#nullable disable
        public ComponentAggregate() { }
#nullable enable

        public void Apply(Marten.Events.Event<Events.ComponentCreated> @event)
        {
            ApplyMeta(@event);
            var data = @event.Data;
            Id = data.AggregateId;
            Information = ComponentInformationAggregateData.From(data.Information);
        }

        public void Apply(Marten.Events.Event<Events.ComponentDeleted> @event)
        {
            ApplyDeleted(@event);
        }

        public override Result<bool, Errors> Validate()
        {
            if (IsVirgin())
                return Result.Combine(
                    base.Validate(),
                    ValidateNull(Information, nameof(Information))
                    );

            return Result.Combine(
                base.Validate(),
                ValidateNonNull(Information, nameof(Information))
                .Bind(_ => Information.Validate())
                );
        }

        public Result<Models.Component, Errors> ToModel()
        {
            var virginResult = ValidateNonVirgin();
            if (virginResult.IsFailure)
                return Result.Failure<Models.Component, Errors>(virginResult.Error);

            var idResult = Infrastructure.ValueObjects.Id.From(Id);
            var informationResult = Information.ToValueObject();
            var timestampResult = Infrastructure.ValueObjects.Timestamp.From(Timestamp);

            return
              Errors.Combine(
                  idResult,
                  informationResult,
                  timestampResult
                  )
              .Bind(_ =>
                  Models.Component.From(
                    id: idResult.Value,
                    information: informationResult.Value,
                    timestamp: timestampResult.Value
                    )
                  );
        }
    }
}