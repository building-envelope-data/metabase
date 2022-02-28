namespace Metabase.GraphQl.Components
{
    public sealed class ComponentGeneralizationOfEdge
        : Edge<Data.Component, ComponentByIdDataLoader>
    {
        public ComponentGeneralizationOfEdge(
            Data.ComponentConcretizationAndGeneralization association
        )
            : base(association.ConcreteComponentId)
        {
        }
    }
}