using System;

namespace Metabase.GraphQl.ComponentGeneralizations
{
    public sealed record RemoveComponentGeneralizationInput(
          Guid GeneralComponentId,
          Guid ConcreteComponentId
        );
}