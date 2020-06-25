namespace Icon.GraphQl
{
    public abstract class AddOrRemoveComponentVariantInput
    {
        public ValueObjects.Id BaseComponentId { get; }
        public ValueObjects.Id VariantComponentId { get; }

        protected AddOrRemoveComponentVariantInput(
            ValueObjects.Id baseComponentId,
            ValueObjects.Id variantComponentId
            )
        {
            BaseComponentId = baseComponentId;
            VariantComponentId = variantComponentId;
        }
    }
}