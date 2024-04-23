using System;

namespace Metabase.Data;

public sealed class ComponentConcretizationAndGeneralization
{
    public Guid GeneralComponentId { get; set; }
    public Component GeneralComponent { get; set; } = default!;

    public Guid ConcreteComponentId { get; set; }
    public Component ConcreteComponent { get; set; } = default!;
}