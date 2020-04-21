using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Guid = System.Guid;
using Commands = Icon.Commands;

namespace Icon.Events
{
    public sealed class ComponentManufacturerAdded : Event
    {
        public static ComponentManufacturerAdded From(
              Guid componentManufacturerId,
              Commands.AddComponentManufacturer command
            )
        {
            return new ComponentManufacturerAdded(
                componentManufacturerId: componentManufacturerId,
                componentId: command.Input.ComponentId,
                institutionId: command.Input.InstitutionId,
                marketingInformation: command.Input.MarketingInformation is null ? null : ComponentManufacturerMarketingInformationEventData.From(command.Input.MarketingInformation),
                creatorId: command.CreatorId
                );
        }

        public Guid ComponentManufacturerId { get; set; }
        public Guid ComponentId { get; set; }
        public Guid InstitutionId { get; set; }
        public ComponentManufacturerMarketingInformationEventData? MarketingInformation { get; set; }

#nullable disable
        public ComponentManufacturerAdded() { }
#nullable enable

        public ComponentManufacturerAdded(
            Guid componentManufacturerId,
            Guid componentId,
            Guid institutionId,
            ComponentManufacturerMarketingInformationEventData? marketingInformation,
            Guid creatorId
            )
          : base(creatorId)
        {
            ComponentManufacturerId = componentManufacturerId;
            ComponentId = componentId;
            InstitutionId = institutionId;
            MarketingInformation = marketingInformation;
            EnsureValid();
        }

        public override Result<bool, Errors> Validate()
        {
            return
              Result.Combine(
                  base.Validate(),
                  ValidateNonEmpty(ComponentManufacturerId, nameof(ComponentManufacturerId)),
                  ValidateNonEmpty(ComponentId, nameof(ComponentId)),
                  ValidateNonEmpty(InstitutionId, nameof(InstitutionId)),
                  MarketingInformation?.Validate() ?? Result.Ok<bool, Errors>(true)
                  );
        }
    }
}