using System.Text.Json.Serialization;
using Metabase.GraphQl;

namespace Metabase.Data;

[JsonPolymorphic(TypeDiscriminatorPropertyName = GraphQlConstants.TypeDiscriminatorPropertyName)]
[JsonDerivedType(typeof(Standard), typeDiscriminator: nameof(Standard))]
[JsonDerivedType(typeof(Publication), typeDiscriminator: nameof(Publication))]
public interface IReference
{
}