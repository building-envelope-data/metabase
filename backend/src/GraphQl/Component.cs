using Guid = System.Guid;
using Models = Icon.Models;

namespace Icon.GraphQl
{
    public sealed class Component
    {
        public static Component FromModel(Models.Component component)
        {
            return new Component
            {
                Id = component.Id,
                Version = component.Version
            };
        }

        public Guid Id { get; set; }
        public int Version { get; set; }

        public Component()
        {
        }
    }
}