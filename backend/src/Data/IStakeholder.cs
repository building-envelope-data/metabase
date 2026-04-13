using System.Text.Json.Serialization;
using Metabase.GraphQl;

namespace Metabase.Data;

[JsonPolymorphic(TypeDiscriminatorPropertyName = GraphQlConstants.TypeDiscriminatorPropertyName)]
[JsonDerivedType(typeof(User), typeDiscriminator: nameof(User))]
[JsonDerivedType(typeof(Institution), typeDiscriminator: nameof(Institution))]
public interface IStakeholder
{
}