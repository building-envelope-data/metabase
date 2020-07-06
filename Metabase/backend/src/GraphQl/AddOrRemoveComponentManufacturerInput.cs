using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public abstract class AddOrRemoveComponentManufacturerInput
    {
        public Id ComponentId { get; }
        public Id InstitutionId { get; }

        public AddOrRemoveComponentManufacturerInput(
            Id componentId,
            Id institutionId
            )
        {
            ComponentId = componentId;
            InstitutionId = institutionId;
        }
    }
}