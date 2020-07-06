using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public abstract class AddOrRemoveComponentConcretizationInput
    {
        public Id GeneralComponentId { get; }
        public Id ConcreteComponentId { get; }

        protected AddOrRemoveComponentConcretizationInput(
            Id generalComponentId,
            Id concreteComponentId
            )
        {
            GeneralComponentId = generalComponentId;
            ConcreteComponentId = concreteComponentId;
        }
    }
}