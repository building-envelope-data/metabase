using System;
using System.Text.Json.Serialization;
using Metabase.Configuration;

namespace Metabase.Data;

[JsonPolymorphic(TypeDiscriminatorPropertyName = GraphQlConfiguration.TypeDiscriminatorPropertyName)]
[JsonDerivedType(typeof(UserMethodDeveloper), typeDiscriminator: nameof(UserMethodDeveloper))]
[JsonDerivedType(typeof(InstitutionMethodDeveloper), typeDiscriminator: nameof(InstitutionMethodDeveloper))]
public interface IMethodDeveloper
{
    public Guid MethodId { get; }
    public Method Method { get; }
    public bool Pending { get; }
}