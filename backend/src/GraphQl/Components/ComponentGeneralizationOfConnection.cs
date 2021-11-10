namespace Metabase.GraphQl.Components
{
    public sealed class ComponentGeneralizationOfConnection
        : Connection<Data.Component, Data.ComponentConcretizationAndGeneralization, ComponentConcretizationsByComponentIdDataLoader, ComponentGeneralizationOfEdge>
    {
        public ComponentGeneralizationOfConnection(
            Data.Component subject
        )
            : base(
                subject,
                x => new ComponentGeneralizationOfEdge(x)
                )
        {
        }
    }
}