using System;

namespace Metabase.GraphQl.ComponentGeneralizations
{
    public record RemoveComponentGeneralizationInput(
          Guid GeneralComponentId,
          Guid ConcreteComponentId
        );
}