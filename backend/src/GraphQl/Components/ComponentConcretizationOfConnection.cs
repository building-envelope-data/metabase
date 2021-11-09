namespace Metabase.GraphQl.Components
{
    public sealed class ComponentConcretizationOfConnection
        : Connection<Data.Component, Data.ComponentConcretizationAndGeneralization, ComponentGeneralizationsByComponentIdDataLoader, ComponentConcretizationOfEdge>
    {
        public ComponentConcretizationOfConnection(
            Data.Component subject
        )
            : base(
                subject,
                x => new ComponentConcretizationOfEdge(x)
                )
        {
        }
    }
}