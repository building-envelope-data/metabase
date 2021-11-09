namespace Metabase.GraphQl.Components
{
    public sealed class ComponentAssembledOfEdge
        : Edge<Data.Component, ComponentByIdDataLoader>
    {
        public ComponentAssembledOfEdge(
            Data.ComponentAssembly association
        )
            : base(association.PartComponentId)
        {
        }
    }
}