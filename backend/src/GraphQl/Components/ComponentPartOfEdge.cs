namespace Metabase.GraphQl.Components
{
    public sealed class ComponentPartOfEdge
        : Edge<Data.Component, ComponentByIdDataLoader>
    {
        public ComponentPartOfEdge(
            Data.ComponentAssembly association
        )
            : base(association.AssembledComponentId)
        {
        }
    }
}