namespace Metabase.GraphQl.Components
{
    public sealed class ComponentManufacturerConnection
        : ForkingConnection<Data.Component, Data.ComponentManufacturer, PendingComponentManufacturersByComponentIdDataLoader, ComponentManufacturersByComponentIdDataLoader, ComponentManufacturerEdge>
    {
        public ComponentManufacturerConnection(
            Data.Component subject,
            bool pending
        )
            : base(
                subject,
                pending,
                x => new ComponentManufacturerEdge(x)
                )
        {
        }
    }
}