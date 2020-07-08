// Inspired by https://github.com/ChilliCream/hotchocolate/blob/10.4.2/src/Core/Types/Types/Scalars/ScalarType.cs
// TODO Once it's available use the generic `ScalarType<.., ..>` as in https://github.com/ChilliCream/hotchocolate/blob/master/src/HotChocolate/Core/src/Types/Types/Scalars/ScalarType.cs, see https://github.com/ChilliCream/hotchocolate/blob/master/src/HotChocolate/Core/src/Types/Types/Scalars/ScalarType~2.cs

using System; // Func
using CSharpFunctionalExtensions;
using HotChocolate.Types;
using Errors = Infrastructure.Errors;
using IValueNode = HotChocolate.Language.IValueNode;
using NameString = HotChocolate.NameString;
using Type = System.Type;
// using TypeResourceHelper = HotChocolate.Properties.TypeResourceHelper;

namespace Metabase.GraphQl
{
    public class WrappingScalarType<TClrType, TWrappedClrType>
        : ScalarType
    {
        private readonly ScalarType _scalarType;
        private readonly Func<TWrappedClrType, Result<TClrType, Errors>> _wrap;
        private readonly Func<TClrType, TWrappedClrType> _unwrap;

        public WrappingScalarType(
            NameString name,
            ScalarType scalarType,
            Func<TWrappedClrType, Result<TClrType, Errors>> wrap,
            Func<TClrType, TWrappedClrType> unwrap
            )
          : base(name)
        {
            _scalarType = scalarType;
            _wrap = wrap;
            _unwrap = unwrap;
        }

        public override Type ClrType => typeof(TClrType);

        public override bool IsInstanceOfType(IValueNode? literal)
        {
            return _scalarType.IsInstanceOfType(literal);
        }

        public override object? ParseLiteral(IValueNode? literal)
        {
            var value = _scalarType.ParseLiteral(literal);
            if (value is null) return null;
            if (value is TWrappedClrType x)
            {
                var wrappingResult = _wrap(x);
                if (wrappingResult.IsSuccess)
                {
                    return wrappingResult.Value;
                }
                else
                {
                    throw new ScalarSerializationException(
                       $"{Name} cannot parse the given literal of type `{literal?.GetType()}` because of the following error(s): {wrappingResult.Error}."
                       );
                }
            }
            throw new ScalarSerializationException(
                $"{Name} cannot parse the given literal of type `{literal?.GetType()}`."
                );
        }

        public override IValueNode? ParseValue(object? value)
        {
            if (value is null)
                return _scalarType.ParseValue(null);
            if (value is TClrType x)
                return _scalarType.ParseValue(_unwrap(x));
            throw new ScalarSerializationException(
                $"{Name} cannot parse the given value of type `{value.GetType()}`."
                );
        }

        public override object? Serialize(object? value)
        {
            if (value is null)
                return _scalarType.Serialize(null);
            if (value is TClrType x)
                return _scalarType.Serialize(_unwrap(x));
            throw new ScalarSerializationException(
                $"{Name} cannot serialize the given value."
                  );
        }

        public override bool TryDeserialize(object? serialized, out object? value)
        {
            if (serialized is TClrType x)
                serialized = _unwrap(x);
            var success = _scalarType.TryDeserialize(serialized, out object deserialized);
            if (deserialized is TWrappedClrType y)
            {
                var result = _wrap(y);
                if (result.IsSuccess)
                {
                    value = result.Value;
                    return true;
                }
                // TODO Is it possible to communicate `result.Error` instead of just `false`
                value = null;
                return false;
            }
            value = null;
            return success;
        }
    }
}