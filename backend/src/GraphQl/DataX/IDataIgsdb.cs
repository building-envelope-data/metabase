using System;

namespace Metabase.GraphQl.DataX;

public interface IDataIgsdb
{
    Guid? Uuid { get; }
    DateTime Timestamp { get; }
    Guid ComponentId { get; }
    string? Name { get; }
    string? Description { get; }
    GetHttpsResourceTreeIgsdb ResourceTree { get; }
}