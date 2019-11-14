using Guid = System.Guid;
using Models = Icon.Models;
using DateTime = System.DateTime;

namespace Icon.GraphQl
{
    public sealed class Component
    {
        public static Component FromModel(
            Models.Component component,
            DateTime requestTimestamp
            )
        {
            return new Component
            {
                Id = component.Id,
                Timestamp = component.Timestamp,
                RequestTimestamp = requestTimestamp
            };
        }

        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime RequestTimestamp { get; set; }

        public Component()
        {
        }
    }
}