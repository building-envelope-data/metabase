using System;
using HotChocolate.Types;

namespace Metabase.GraphQl.DataX
{
    [InterfaceType("Data")]
    public interface IData
    {
        Guid Uuid { get; set; }
        DateTime Timestamp { get; set; }
        Guid ComponentId { get; set; }
        string? Name { get; set; }
        // string? Description { get; set; }
        // List<string> Warnings { get; set; }
        // Guid CreatorId { get; set; }
        // DateTime CreatedAt { get; set; }
        AppliedMethod AppliedMethod { get; set; }
        // List<DataApproval> Approvals { get; set; }
        // List<GetHttpsResource> Resources { get; set; }
        GetHttpsResourceTree ResourceTree { get; set; }
        // ResponseApproval Approval { get; set; }
        // string Locale { get; set; }
    }
}