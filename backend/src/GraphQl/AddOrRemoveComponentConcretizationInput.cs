namespace Icon.GraphQl
{
    public abstract class AddOrRemoveComponentConcretizationInput
    {
        public ValueObjects.Id GeneralComponentId { get; }
        public ValueObjects.Id ConcreteComponentId { get; }

        protected AddOrRemoveComponentConcretizationInput(
            ValueObjects.Id generalComponentId,
            ValueObjects.Id concreteComponentId
            )
        {
            GeneralComponentId = generalComponentId;
            ConcreteComponentId = concreteComponentId;
        }
    }
}