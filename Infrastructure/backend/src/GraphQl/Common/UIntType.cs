using HotChocolate.Language;
using HotChocolate.Types;
using HotChocolate.Properties;
using Convert = System.Convert;
using HotChocolate;

namespace Infrastructure.GraphQl.Common
{
    // TODO Just map C# `uint` (or `uint`) to GraphQl `Int`
    // Inspired by https://github.com/ChilliCream/hotchocolate/blob/main/src/HotChocolate/Core/src/Types/Types/Scalars/LongType.cs
    public sealed class UIntType
        : IntegerTypeBase<uint>
    {
        private static readonly string ScalarName = "UInt";
        private static readonly string ScalarDescription = "The `UInt` scalar represents a 32-bit unsigned integer type.";

        public UIntType()
            : this(uint.MinValue, uint.MaxValue)
        {
        }

        public UIntType(uint min, uint max)
            : this(ScalarName, ScalarDescription, min, max)
        {
        }

        public UIntType(NameString name)
            : this(name, uint.MinValue, uint.MaxValue)
        {
        }

        public UIntType(NameString name, uint min, uint max)
            : base(name, min, max, BindingBehavior.Implicit)
        {
        }

        public UIntType(NameString name, string description, uint min, uint max)
            : base(name, min, max, BindingBehavior.Implicit)
        {
            Description = description;
        }

        protected override uint ParseLiteral(IntValueNode valueSyntax)
        {
            return Convert.ToUInt32(valueSyntax.ToInt64());
        }

        protected override IntValueNode ParseValue(uint runtimeValue)
        {
            return new IntValueNode(Convert.ToInt64(runtimeValue));
        }
    }
}
