namespace Metabase.GraphQl.Components
{
    public sealed class ComponentVariantOfConnection
        : Connection<Data.Component, Data.ComponentVariant, ComponentVariantOfByComponentIdDataLoader, ComponentVariantOfEdge>
    {
        public ComponentVariantOfConnection(
            Data.Component subject
        )
            : base(
                subject,
                x => new ComponentVariantOfEdge(x)
                )
        {
        }
    }
}