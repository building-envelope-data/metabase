using Guid = System.Guid;
using Models = Icon.Models;
using DateTime = System.DateTime;

namespace Icon.GraphQl
{
    public sealed class Component
    {
        public static Component FromModel(Models.Component component)
        {
            return new Component
            {
                Id = component.Id,
                Timestamp = component.Timestamp
            };
        }

        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }

        public Component()
        {
        }
    }
}