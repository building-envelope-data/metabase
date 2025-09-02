using System.Text.Json.Serialization;
using Metabase.Configuration;

namespace Metabase.Data;

[JsonPolymorphic(TypeDiscriminatorPropertyName = GraphQlConfiguration.TypeDiscriminatorPropertyName)]
[JsonDerivedType(typeof(Standard), typeDiscriminator: nameof(Standard))]
[JsonDerivedType(typeof(Publication), typeDiscriminator: nameof(Publication))]
public interface IReference
{
}