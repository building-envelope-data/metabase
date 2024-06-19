using System;

namespace Metabase.GraphQl.DataX;

public interface IDataIgsdb
{
    string Id { get; }
    Guid? Uuid { get; }
    DateTime Timestamp { get; }
    Guid ComponentId { get; }
    string? Name { get; }
    string? Description { get; }
    GetHttpsResourceTreeIgsdb ResourceTree { get; }
}