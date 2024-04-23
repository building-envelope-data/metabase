using System;

namespace Metabase.GraphQl.ComponentGeneralizations
{
    public sealed record AddComponentGeneralizationInput(
        Guid GeneralComponentId,
        Guid ConcreteComponentId
    );
}