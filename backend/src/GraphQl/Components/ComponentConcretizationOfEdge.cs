namespace Metabase.GraphQl.Components
{
    public sealed class ComponentConcretizationOfEdge
        : Edge<Data.Component, ComponentByIdDataLoader>
    {
        public ComponentConcretizationOfEdge(
            Data.ComponentConcretizationAndGeneralization association
        )
            : base(association.GeneralComponentId)
        {
        }
    }
}