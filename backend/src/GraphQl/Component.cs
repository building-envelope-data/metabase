using Guid = System.Guid;
using Models = Icon.Models;
using DateTime = System.DateTime;

namespace Icon.GraphQl
{
    public sealed class Component
      : Node
    {
        public static Component FromModel(
            Models.Component model,
            DateTime requestTimestamp
            )
        {
          return new Component(
              model.Id,
              information: ComponentInformation.FromModel(model.Information),
              timestamp: model.Timestamp,
              requestTimestamp: requestTimestamp
              );
        }

        public ComponentInformation Information { get; set; }

        public Component() { }

        public Component(
            Guid id,
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
          Information = information;
      }
    }
}