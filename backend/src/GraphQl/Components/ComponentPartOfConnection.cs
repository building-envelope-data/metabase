namespace Metabase.GraphQl.Components
{
    public sealed class ComponentPartOfConnection
        : Connection<Data.Component, Data.ComponentAssembly, ComponentPartOfByComponentIdDataLoader, ComponentPartOfEdge>
    {
        public ComponentPartOfConnection(
            Data.Component subject
        )
            : base(
                subject,
                x => new ComponentPartOfEdge(x)
                )
        {
        }
    }
}