using System;

namespace Metabase.GraphQl.ComponentGeneralizations
{
    public record AddComponentGeneralizationInput(
          Guid GeneralComponentId,
          Guid ConcreteComponentId
        );
}