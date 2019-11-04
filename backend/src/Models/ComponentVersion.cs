using Guid = System.Guid;

namespace Icon.Models
{
  public sealed class ComponentVersion
  {
    public Guid Id { get; }
    public Guid ComponentId { get; }
    public int Version { get; }

    public ComponentVersion(Guid id, Guid componentId, int version)
    {
      Id = id;
      ComponentId = componentId;
      Version = version;
    }
  }
}
