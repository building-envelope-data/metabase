using Infrastructure.ValueObjects;
using Guid = System.Guid;
using UuidType = HotChocolate.Types.UuidType;

namespace Infrastructure.GraphQl
{
    public sealed class NonEmptyUuidType
        : WrappingScalarType<Id, Guid>
    {
        public NonEmptyUuidType()
          : base(
              "NonEmptyUuid",
              new UuidType(),
              guid => ValueObjects.Id.From(guid),
              id => id.Value
              )
        {
        }
    }
}