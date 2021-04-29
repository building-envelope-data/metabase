namespace Metabase.GraphQl.Components
{
    public sealed class ComponentManufacturerConnection
        : Connection<Data.Component, Data.ComponentManufacturer, ComponentManufacturersByComponentIdDataLoader, ComponentManufacturerEdge>
    {
        public ComponentManufacturerConnection(
            Data.Component subject
        )
            : base(
                subject,
                x => new ComponentManufacturerEdge(x)
                )
        {
        }
    }
}