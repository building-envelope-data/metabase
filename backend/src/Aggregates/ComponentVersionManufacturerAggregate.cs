using Icon;
using System;
using Icon.Infrastructure.Aggregate;
using Marten.Schema;
/* using DateInterval = NodaTime.DateInterval; */
using DateTime = System.DateTime;
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

        public override bool IsValid()
        {
            return
              base.IsValid() &&
              (
                IsVirgin() &&
                ComponentVersionId == Guid.Empty &&
                InstitutionId == Guid.Empty &&
                MarketingInformation is null
              )
              ||
              (
                  !IsVirgin() &&
                  ComponentVersionId != Guid.Empty &&
                  InstitutionId != Guid.Empty &&
                  (MarketingInformation?.IsValid() ?? false)
              );
        }

        public Models.ComponentVersionManufacturer ToModel()
        {
            EnsureNotVirgin();
            EnsureValid();
            return new Models.ComponentVersionManufacturer(
              id: Id,
              componentVersionId: ComponentVersionId,
              institutionId: InstitutionId,
              marketingInformation: MarketingInformation?.ToModel(),
              timestamp: Timestamp
            );
        }
    }
}