using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using HotChocolate;

namespace Metabase.GraphQl.DataX
{
    public abstract class Data : IData {
      // public string Id { get; set; }
      public Guid Uuid { get; set; }
      public DateTime Timestamp { get; set; }
      // public string Locale { get; set; }
      // public Guid DatabaseId { get; set; }
      public Guid ComponentId { get; set; }
      public string? Name { get; set; }
      // public string? Description { get; set; }
      // public List<string> Warnings { get; set; }
      // public Guid CreatorId { get; set; }
      // public DateTime CreatedAt { get; set; }
      public AppliedMethod AppliedMethod { get; set; }
      // public List<GetHttpsResource> Resources { get; set; }
      public GetHttpsResourceTree ResourceTree { get; set; }
      // public List<DataApproval> Approvals { get; set; }
      // public ResponseApproval Approval { get; set; }

      // [GraphQLIgnore]
      // [JsonExtensionData]
      // public Dictionary<string, JsonElement>? ExtensionData { get; set; }
    }
}