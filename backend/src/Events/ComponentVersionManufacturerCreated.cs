using Guid = System.Guid;
using System.Threading.Tasks;
using System.Collections.Generic;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure;
using Icon.Infrastructure.Command;
using Icon.Events;
using Icon.Infrastructure.Aggregate;
using Commands = Icon.Commands;
using DateTime = System.DateTime;

namespace Icon.Events
{
    public sealed class ComponentVersionManufacturerCreated : Event
    {
        public static ComponentVersionManufacturerCreated From(
              Guid componentVersionManufacturerId,
              Commands.CreateComponentVersionManufacturer command
            )
        {
            return new ComponentVersionManufacturerCreated(
                componentVersionManufacturerId: componentVersionManufacturerId,
                componentVersionId: command.ComponentVersionId,
                institutionId: command.InstitutionId,
                marketingInformation: ComponentVersionManufacturerMarketingInformationEventData.From(command.MarketingInformation),
                creatorId: command.CreatorId
                );
        }

        public Guid ComponentVersionManufacturerId { get; set; }
        public Guid ComponentVersionId { get; set; }
        public Guid InstitutionId { get; set; }
        public ComponentVersionManufacturerMarketingInformationEventData MarketingInformation { get; set; }

        public ComponentVersionManufacturerCreated() { }

        public ComponentVersionManufacturerCreated(
            Guid componentVersionManufacturerId,
            Guid componentVersionId,
            Guid institutionId,
            ComponentVersionManufacturerMarketingInformationEventData marketingInformation,
            Guid creatorId
            )
          : base(creatorId)
        {
            ComponentVersionManufacturerId = componentVersionManufacturerId;
            ComponentVersionId = componentVersionId;
            InstitutionId = institutionId;
            MarketingInformation = marketingInformation;
        }

        public override bool IsValid()
        {
            return ComponentVersionManufacturerId != Guid.Empty &&
              ComponentVersionId != Guid.Empty &&
              InstitutionId != Guid.Empty &&
              MarketingInformation.IsValid();
        }
    }
}