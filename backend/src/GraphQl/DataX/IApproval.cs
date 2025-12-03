using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using HotChocolate.Types;
using Metabase.Configuration;
using NodaTime;

namespace Metabase.GraphQl.DataX;

[InterfaceType("Approval")]
public interface IApproval
{
    OffsetDateTime Timestamp { get; }
    string Signature { get; }
    string KeyFingerprint { get; }
    string Query { get; }
    JsonElement Variables { get; }
    string Message { get; }
    Guid ApproverId { get; }
}