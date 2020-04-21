using Guid = System.Guid;
using Models = Icon.Models;
using DateTime = System.DateTime;

namespace Icon.GraphQl
{
    public sealed class ComponentVersion
      : NodeBase
    {
        public static ComponentVersion FromModel(
            Models.ComponentVersion model,
            ValueObjects.Timestamp requestTimestamp
            )
        {
            return new ComponentVersion(
                model.Id,
                componentId: model.ComponentId,
                information: ComponentInformation.FromModel(model.Information),
                timestamp: model.Timestamp,
                requestTimestamp: requestTimestamp
                );
        }

        public Guid ComponentId { get; }
        public ComponentInformation Information { get; }

        public ComponentVersion(
            Guid id,
            Guid componentId,
            ComponentInformation information,
            DateTime timestamp,
            DateTime requestTimestamp
            )
          : base(
              id: id,
              timestamp: timestamp,
              requestTimestamp: requestTimestamp
              )
        {
            ComponentId = componentId;
            Information = information;
        }
    }
}