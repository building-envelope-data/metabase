using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Icon.ValueObjects
{
    public sealed class AddComponentManufacturerInput
      : AddManyToManyAssociationInput
    {
        public static Result<AddComponentManufacturerInput, Errors> From(
            Id componentId,
            Id institutionId,
            ComponentManufacturerMarketingInformation? marketingInformation
            )
        {
            return
              Result.Ok<AddComponentManufacturerInput, Errors>(
                  new AddComponentManufacturerInput(
                    componentId: componentId,
                    institutionId: institutionId,
                    marketingInformation: marketingInformation
                    )
                  );
        }

        public Id ComponentId { get => ParentId; }
        public Id InstitutionId { get => AssociateId; }
        public ComponentManufacturerMarketingInformation? MarketingInformation { get; }

        private AddComponentManufacturerInput(
            Id componentId,
            Id institutionId,
            ComponentManufacturerMarketingInformation? marketingInformation
            )
          : base(
              parentId: componentId,
              associateId: institutionId
              )
        {
            MarketingInformation = marketingInformation;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            foreach (var component in base.GetEqualityComponents())
            {
                yield return component;
            }
            yield return MarketingInformation;
        }
    }
}