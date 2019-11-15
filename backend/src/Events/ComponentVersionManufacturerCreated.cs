using Guid = System.Guid;
using System.Threading.Tasks;
using System.Collections.Generic;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure;
using Icon.Infrastructure.Command;
using Icon.Infrastructure.Event;
using Icon.Infrastructure.Aggregate;
using Commands = Icon.Commands;
using DateTime = System.DateTime;

namespace Icon.Events
{
    public sealed class ComponentVersionManufacturerCreated : EventBase
    {
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

        public ComponentVersionManufacturerCreated(
            Guid componentVersionManufacturerId,
            Commands.CreateComponentVersionManufacturer command
            )
          : this(
              componentVersionManufacturerId: componentVersionManufacturerId,
              componentVersionId: command.ComponentVersionId,
              institutionId: command.InstitutionId,
              marketingInformation: new ComponentVersionManufacturerMarketingInformationEventData(command.MarketingInformation),
              creatorId: command.CreatorId
              )
      { }
    }
}