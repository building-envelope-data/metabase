using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using HotChocolate;
using HotChocolate.Types;
using NodaTime;

namespace Metabase.GraphQl.DataX;

[InterfaceType("Data")]
[JsonPolymorphic(TypeDiscriminatorPropertyName = GraphQlConstants.TypeDiscriminatorPropertyName)]
[JsonDerivedType(typeof(CalorimetricData), typeDiscriminator: nameof(CalorimetricData))]
[JsonDerivedType(typeof(GeometricData), typeDiscriminator: nameof(GeometricData))]
[JsonDerivedType(typeof(HygrothermalData), typeDiscriminator: nameof(HygrothermalData))]
[JsonDerivedType(typeof(LifeCycleData), typeDiscriminator: nameof(LifeCycleData))]
[JsonDerivedType(typeof(OpticalData), typeDiscriminator: nameof(OpticalData))]
[JsonDerivedType(typeof(PhotovoltaicData), typeDiscriminator: nameof(PhotovoltaicData))]
public interface IData
{
    Guid Uuid { get; }
    DataKind Kind { get; }
    OffsetDateTime Timestamp { get; }
    Guid ComponentId { get; }
    string? Name { get; }
    Guid DatabaseId { get; }
    string? Description { get; }
    IReadOnlyList<string> Warnings { get; }
    Guid CreatorId { get; }
    OffsetDateTime CreatedAt { get; }
    AppliedMethod AppliedMethod { get; }
    IReadOnlyList<DataApproval> Approvals { get; }
    IReadOnlyList<GetHttpsResource> Resources { get; }
    GetHttpsResourceTree ResourceTree { get; }
    // ResponseApproval Approval { get; }

    [GraphQLType<NonNullType<LocaleType>>]
    string Locale { get; }
}