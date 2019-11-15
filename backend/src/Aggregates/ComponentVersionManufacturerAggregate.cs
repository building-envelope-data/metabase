using System;
using Icon.Infrastructure.Aggregate;
using Marten.Schema;
/* using DateInterval = NodaTime.DateInterval; */
using DateTime = System.DateTime;
using Events = Icon.Events;

namespace Icon.Aggregates
{
    public sealed class ComponentVersionManufacturerAggregate : EventSourcedAggregate
    {
        [ForeignKey(typeof(ComponentVersionAggregate))]
        public Guid ComponentVersionId { get; set; }

        /* [ForeignKey(typeof(InstitutionAggregate))] */
        public Guid InstitutionId { get; set; }

        public ComponentVersionManufacturerMarketingInformationAggregateData MarketingInformation { get; set; }

        public ComponentVersionManufacturerAggregate() { }

        private void Apply(Marten.Events.Event<Events.ComponentVersionManufacturerCreated> @event)
        {
            ApplyMeta(@event);
            var data = @event.Data;
            Id = data.ComponentVersionManufacturerId;
            ComponentVersionId = data.ComponentVersionId;
            MarketingInformation = new ComponentVersionManufacturerMarketingInformationAggregateData(data.MarketingInformation);
        }

        public Models.ComponentVersionManufacturer ToModel()
        {
            return new Models.ComponentVersionManufacturer(
              id: Id,
              componentVersionId: ComponentVersionId,
              institutionId: InstitutionId,
              marketingInformation: MarketingInformation.ToModel(),
              timestamp: Timestamp
            );
        }
    }
}