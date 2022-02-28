namespace Metabase.GraphQl.Components
{
    public sealed class ComponentVariantOfEdge
        : Edge<Data.Component, ComponentByIdDataLoader>
    {
        public ComponentVariantOfEdge(
            Data.ComponentVariant association
        )
            : base(association.OfComponentId)
        {
        }
    }
}