using System;
using System.Text.Json;
using HotChocolate.Types;

namespace Metabase.GraphQl.DataX;

[InterfaceType("Approval")]
public interface IApproval
{
    DateTime Timestamp { get; }
    string Signature { get; }
    string KeyFingerprint { get; }
    string Query { get; }
    JsonElement Variables { get; }
    string Message { get; }
}