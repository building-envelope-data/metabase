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
      : EventSourcedAggregate
    {
        [ForeignKey(typeof(ComponentVersionAggregate))]
        public Guid ComponentVersionId { get; set; }

        /* [ForeignKey(typeof(InstitutionAggregate))] */
        public Guid InstitutionId { get; set; }

        public ComponentVersionManufacturerMarketingInformationAggregateData? MarketingInformation { get; set; }

#nullable disable
        public ComponentVersionManufacturerAggregate() { }
#nullable enable

        private void Apply(Marten.Events.Event<Events.ComponentVersionManufacturerCreated> @event)
        {
            ApplyMeta(@event);
            var data = @event.Data;
            Id = data.ComponentVersionManufacturerId.NotEmpty();
            ComponentVersionId = data.ComponentVersionId.NotEmpty();
            MarketingInformation = data.MarketingInformation is null ? null : ComponentVersionManufacturerMarketingInformationAggregateData.From(data.MarketingInformation);
        }

        public override Result<bool, Errors> Validate()
        {
            if (IsVirgin())
              return
                Result.Combine(
                    base.Validate(),
                    ValidateEmpty(ComponentId),
                    ValidateEmpty(InstitutionId),
                    ValidateNull(MarketingInformation)
                    );

            else
              return
                Result.Combine(
                    base.Validate(),
                    ValidateNonEmpty(ComponentId, nameof(ComponentId)),
                    ValidateNonEmpty(InstitutionId, nameof(InstitutionId)),
                    MarketingInformation.Validate()
                    );
        }

        public Result<Models.ComponentVersionManufacturer, Errors> ToModel()
        {
          var virginResult = ValidateNonVirgin();
          if (virginResult.IsFailure)
            return Result.Failure<ValueObjects.ComponentVersionManufacturer, Errors>(virginResult.Error);

          var idResult = ValueObjects.Id.From(Id);
          var componentVersionIdResult = ValueObjects.Id.From(componentVersionId);
          var institutionIdResult = ValueObjects.Id.From(institutionId);
          var marketingInformationResult = MarketingInformation.ToValueObject();
          var timestampResult = ValueObjects.Timestamp.From(Timestamp);

          var errors = Errors.From(
              idResult,
              componentVersionIdResult,
              institutionIdResult,
              marketingInformationResult,
              timestampResult
              );

          if (!errors.IsEmpty())
            return Result.Failure<ValueObjects.ComponentVersionManufacturer, Errors>(errors);

          return Models.ComponentVersionManufacturer.From(
              id: idResult.Value,
              componentVersionId: componentVersionIdResult.Value,
              institutionId: institutionIdResult.Value,
              marketingInformation: marketingInformationResult.Value,
              timestamp: timestampResult.Value
              );
        }
    }
}