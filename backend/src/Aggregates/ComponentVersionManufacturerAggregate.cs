using Icon;
using System;
using Icon.Infrastructure.Aggregate;
using Marten.Schema;
/* using DateInterval = NodaTime.DateInterval; */
using DateTime = System.DateTime;
using CSharpFunctionalExtensions;
using Events = Icon.Events;

namespace Icon.Aggregates
{
    public sealed class ComponentVersionManufacturerAggregate
      : EventSourcedAggregate, IConvertible<Models.ComponentVersionManufacturer>
    {
        [ForeignKey(typeof(ComponentVersionAggregate))]
        public Guid ComponentVersionId { get; set; }

        /* [ForeignKey(typeof(InstitutionAggregate))] */
        public Guid InstitutionId { get; set; }

        public ComponentManufacturerMarketingInformationAggregateData? MarketingInformation { get; set; }

#nullable disable
        public ComponentVersionManufacturerAggregate() { }
#nullable enable

        private void Apply(Marten.Events.Event<Events.ComponentVersionManufacturerCreated> @event)
        {
            ApplyMeta(@event);
            var data = @event.Data;
            Id = data.ComponentVersionManufacturerId.NotEmpty();
            ComponentVersionId = data.ComponentVersionId.NotEmpty();
            MarketingInformation = data.MarketingInformation is null ? null : ComponentManufacturerMarketingInformationAggregateData.From(data.MarketingInformation);
        }

        public override Result<bool, Errors> Validate()
        {
            if (IsVirgin())
                return
                  Result.Combine<bool, Errors>(
                      base.Validate(),
                      ValidateEmpty(ComponentVersionId, nameof(ComponentVersionId)),
                      ValidateEmpty(InstitutionId, nameof(InstitutionId)),
                      ValidateNull(MarketingInformation, nameof(MarketingInformation))
                      );

            return
              Result.Combine<bool, Errors>(
                  base.Validate(),
                  ValidateNonEmpty(ComponentVersionId, nameof(ComponentVersionId)),
                  ValidateNonEmpty(InstitutionId, nameof(InstitutionId)),
                  MarketingInformation?.Validate() ?? Result.Ok<bool, Errors>(true)
                  );
        }

        public Result<Models.ComponentVersionManufacturer, Errors> ToModel()
        {
            var virginResult = ValidateNonVirgin();
            if (virginResult.IsFailure)
                return Result.Failure<Models.ComponentVersionManufacturer, Errors>(virginResult.Error);

            var idResult = ValueObjects.Id.From(Id);
            var componentVersionIdResult = ValueObjects.Id.From(ComponentVersionId);
            var institutionIdResult = ValueObjects.Id.From(InstitutionId);
            var marketingInformationResult = MarketingInformation?.ToValueObject();
            var timestampResult = ValueObjects.Timestamp.From(Timestamp);

            return
              Errors.CombineExistent(
                  idResult,
                  componentVersionIdResult,
                  institutionIdResult,
                  marketingInformationResult,
                  timestampResult
                  )
              .Bind(_ =>
                  Models.ComponentVersionManufacturer.From(
                    id: idResult.Value,
                    componentVersionId: componentVersionIdResult.Value,
                    institutionId: institutionIdResult.Value,
                    marketingInformation: marketingInformationResult?.Value,
                    timestamp: timestampResult.Value
                    )
                  );
        }
    }
}