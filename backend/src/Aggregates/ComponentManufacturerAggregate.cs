using CSharpFunctionalExtensions;
using Icon.Infrastructure.Aggregate;
using Marten.Schema;
using Guid = System.Guid;

namespace Icon.Aggregates
{
    public sealed class ComponentManufacturerAggregate
      : EventSourcedAggregate, IManyToManyAssociationAggregate, IConvertible<Models.ComponentManufacturer>
    {
        [ForeignKey(typeof(ComponentAggregate))]
        public Guid ComponentId { get; set; }

        [ForeignKey(typeof(InstitutionAggregate))]
        public Guid InstitutionId { get; set; }

        public Guid ParentId { get => ComponentId; }
        public Guid AssociateId { get => InstitutionId; }

        public ComponentManufacturerMarketingInformationAggregateData? MarketingInformation { get; set; }

#nullable disable
        public ComponentManufacturerAggregate() { }
#nullable enable

        private void Apply(Marten.Events.Event<Events.ComponentManufacturerAdded> @event)
        {
            ApplyMeta(@event);
            var data = @event.Data;
            Id = data.AggregateId.NotEmpty();
            ComponentId = data.ComponentId.NotEmpty();
            InstitutionId = data.InstitutionId.NotEmpty();
            MarketingInformation = data.MarketingInformation is null ? null : ComponentManufacturerMarketingInformationAggregateData.From(data.MarketingInformation);
        }

        private void Apply(Marten.Events.Event<Events.ComponentManufacturerRemoved> @event)
        {
            ApplyDeleted(@event);
        }

        public override Result<bool, Errors> Validate()
        {
            if (IsVirgin())
                return
                  Result.Combine<bool, Errors>(
                      base.Validate(),
                      ValidateEmpty(ComponentId, nameof(ComponentId)),
                      ValidateEmpty(InstitutionId, nameof(InstitutionId)),
                      ValidateNull(MarketingInformation, nameof(MarketingInformation))
                      );

            return
              Result.Combine<bool, Errors>(
                  base.Validate(),
                  ValidateNonEmpty(ComponentId, nameof(ComponentId)),
                  ValidateNonEmpty(InstitutionId, nameof(InstitutionId)),
                  MarketingInformation?.Validate() ?? Result.Ok<bool, Errors>(true)
                  );
        }

        public Result<Models.ComponentManufacturer, Errors> ToModel()
        {
            var virginResult = ValidateNonVirgin();
            if (virginResult.IsFailure)
                return Result.Failure<Models.ComponentManufacturer, Errors>(virginResult.Error);

            var idResult = ValueObjects.Id.From(Id);
            var componentIdResult = ValueObjects.Id.From(ComponentId);
            var institutionIdResult = ValueObjects.Id.From(InstitutionId);
            var marketingInformationResult = MarketingInformation?.ToValueObject();
            var timestampResult = ValueObjects.Timestamp.From(Timestamp);

            return
              Errors.CombineExistent(
                  idResult,
                  componentIdResult,
                  institutionIdResult,
                  marketingInformationResult,
                  timestampResult
                  )
              .Bind(_ =>
                  Models.ComponentManufacturer.From(
                    id: idResult.Value,
                    componentId: componentIdResult.Value,
                    institutionId: institutionIdResult.Value,
                    marketingInformation: marketingInformationResult?.Value,
                    timestamp: timestampResult.Value
                    )
                  );
        }
    }
}