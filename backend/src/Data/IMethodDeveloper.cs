using System;
using System.Text.Json.Serialization;
using Metabase.GraphQl;

namespace Metabase.Data;

[JsonPolymorphic(TypeDiscriminatorPropertyName = GraphQlConstants.TypeDiscriminatorPropertyName)]
[JsonDerivedType(typeof(UserMethodDeveloper), typeDiscriminator: nameof(UserMethodDeveloper))]
[JsonDerivedType(typeof(InstitutionMethodDeveloper), typeDiscriminator: nameof(InstitutionMethodDeveloper))]
public interface IMethodDeveloper
: IAuditable, IAssociation
{
    public Guid MethodId { get; }
    public Method Method { get; }
    public bool Pending { get; }
}