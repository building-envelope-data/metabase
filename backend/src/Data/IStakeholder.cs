using System.Text.Json.Serialization;
using Metabase.Configuration;

namespace Metabase.Data;

[JsonPolymorphic(TypeDiscriminatorPropertyName = GraphQlConfiguration.TypeDiscriminatorPropertyName)]
[JsonDerivedType(typeof(User), typeDiscriminator: nameof(User))]
[JsonDerivedType(typeof(Institution), typeDiscriminator: nameof(Institution))]
public interface IStakeholder
{
}