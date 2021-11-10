namespace Metabase.GraphQl.Components
{
    public sealed class ComponentAssembledOfConnection
        : Connection<Data.Component, Data.ComponentAssembly, ComponentPartsByComponentIdDataLoader, ComponentAssembledOfEdge>
    {
        public ComponentAssembledOfConnection(
            Data.Component subject
        )
            : base(
                subject,
                x => new ComponentAssembledOfEdge(x)
                )
        {
        }
    }
}