using Guid = System.Guid;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
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
                marketingInformation: command.MarketingInformation is null ? null : ComponentManufacturerMarketingInformationEventData.From(command.MarketingInformation.NotNull()),
                creatorId: command.CreatorId
                );
        }

        public Guid ComponentVersionManufacturerId { get; set; }
        public Guid ComponentVersionId { get; set; }
        public Guid InstitutionId { get; set; }
        public ComponentManufacturerMarketingInformationEventData? MarketingInformation { get; set; }

#nullable disable
        public ComponentVersionManufacturerCreated() { }
#nullable enable

        public ComponentVersionManufacturerCreated(
            Guid componentVersionManufacturerId,
            Guid componentVersionId,
            Guid institutionId,
            ComponentManufacturerMarketingInformationEventData? marketingInformation,
            Guid creatorId
            )
          : base(creatorId)
        {
            ComponentVersionManufacturerId = componentVersionManufacturerId;
            ComponentVersionId = componentVersionId;
            InstitutionId = institutionId;
            MarketingInformation = marketingInformation;
            EnsureValid();
        }

        public override Result<bool, Errors> Validate()
        {
            return
              Result.Combine(
                  base.Validate(),
                  ValidateNonEmpty(ComponentVersionManufacturerId, nameof(ComponentVersionManufacturerId)),
                  ValidateNonEmpty(ComponentVersionId, nameof(ComponentVersionId)),
                  ValidateNonEmpty(InstitutionId, nameof(InstitutionId)),
                  MarketingInformation?.Validate() ?? Result.Ok<bool, Errors>(true)
                  );
        }
    }
}