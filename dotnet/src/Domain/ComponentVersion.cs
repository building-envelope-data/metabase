using System;
using Icon.Infrastructure.Aggregate;
/* using Icon.Domain.ComponentVersion.Event; */

namespace Icon.Domain
{
    public class ComponentVersion : EventSourcedAggregate
    {
      /* public static ComponentVersion CreateNewComponentVersion(User creator) */
      /*   { */
      /*       var component = new ComponentVersion(); */
      /*       var @event = new NewComponentVersionCreated */
      /*       { */
      /*           ComponentVersionId = Guid.NewGuid(), */
      /*           CreatorId = creator.Id, */
      /*       }; */
      /*       component.Apply(@event); */
      /*       component.Append(@event); */
      /*       return component; */
      /*   } */

      /* public ComponentVersion() */
      /* { */
      /* } */

      /*   public void Apply(NewComponentVersionCreated @event) */
      /*   { */
      /*       Id = @event.ComponentVersionId; */
      /*   } */
    }
}
