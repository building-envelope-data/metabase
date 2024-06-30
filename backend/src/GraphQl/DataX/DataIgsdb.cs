using System;

namespace Metabase.GraphQl.DataX;

public abstract class DataIgsdb
    : IDataIgsdb
{
    protected DataIgsdb(
        string id,
        Guid? uuid,
        DateTime timestamp,
        Guid componentId,
        string? name,
        string? description,
        GetHttpsResourceTreeIgsdb resourceTree
    )
    {
        Id = id;
        Uuid = uuid;
        Timestamp = timestamp;
        ComponentId = componentId;
        Name = name;
        Description = description;
        ResourceTree = resourceTree;
    }

    public string Id { get; }
    public Guid? Uuid { get; }
    public DateTime Timestamp { get; }
    public Guid ComponentId { get; }
    public string? Name { get; }
    public string? Description { get; }
    public GetHttpsResourceTreeIgsdb ResourceTree { get; }
}