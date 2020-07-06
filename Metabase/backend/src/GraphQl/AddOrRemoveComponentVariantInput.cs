using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public abstract class AddOrRemoveComponentVariantInput
    {
        public Id BaseComponentId { get; }
        public Id VariantComponentId { get; }

        protected AddOrRemoveComponentVariantInput(
            Id baseComponentId,
            Id variantComponentId
            )
        {
            BaseComponentId = baseComponentId;
            VariantComponentId = variantComponentId;
        }
    }
}