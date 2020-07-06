using CSharpFunctionalExtensions;
using Infrastructure.Models;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Models
{
    public sealed class ComponentManufacturer
      : Model, IManyToManyAssociation
    {
        public Id ComponentId { get; }
        public Id InstitutionId { get; }
        public ValueObjects.ComponentManufacturerMarketingInformation? MarketingInformation { get; }

        public Id ParentId { get => ComponentId; }
        public Id AssociateId { get => InstitutionId; }

        private ComponentManufacturer(
            Id id,
            Id componentId,
            Id institutionId,
            ValueObjects.ComponentManufacturerMarketingInformation? marketingInformation,
            Timestamp timestamp
            )
          : base(id, timestamp)
        {
            ComponentId = componentId;
            InstitutionId = institutionId;
            MarketingInformation = marketingInformation;
        }

        public static Result<ComponentManufacturer, Errors> From(
            Id id,
            Id componentId,
            Id institutionId,
            ValueObjects.ComponentManufacturerMarketingInformation? marketingInformation,
            Timestamp timestamp
            )
        {
            return
                Result.Ok<ComponentManufacturer, Errors>(
                    new ComponentManufacturer(
                        id: id,
                        componentId: componentId,
                        institutionId: institutionId,
                        marketingInformation: marketingInformation,
                        timestamp: timestamp
                        )
                    );
        }
    }
}