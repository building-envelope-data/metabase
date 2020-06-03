using Guid = System.Guid;
using UuidType = HotChocolate.Types.UuidType;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public sealed class NonEmptyUuidType
        : WrappingScalarType<ValueObjects.Id, Guid>
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