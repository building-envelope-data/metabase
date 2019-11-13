using Guid = System.Guid;

namespace Icon.GraphQl
{
    public sealed class ComponentVersionInput
    {
        public Guid ComponentId { get; set; }

        public ComponentVersionInput()
        {
        }
    }
}