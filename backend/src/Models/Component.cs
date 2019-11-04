using Guid = System.Guid;

namespace Icon.Models
{
  public sealed class Component
  {
    public Guid Id { get; }
    public int Version { get; }

    public Component(Guid id, int version)
    {
      Id = id;
      Version = version;
    }
  }
}