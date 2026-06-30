using System;
using System.Text.Json;
using HotChocolate;
using HotChocolate.Language;
using HotChocolate.Types;

namespace Metabase.GraphQl;

// https://github.com/ChilliCream/graphql-platform/blob/main/src/HotChocolate/Core/src/Types/Utilities/ThrowHelper.cs
public static class GraphQlThrowHelper
{
    public static LeafCoercionException ScalarCannotCoerceInputLiteral(
        ITypeDefinition scalarType,
        IValueNode? valueLiteral,
        Exception? error = null)
    {
        valueLiteral ??= NullValueNode.Default;
        var errorBuilder =
            ErrorBuilder.New()
                .SetMessage(
                    GraphQlTypeResources.ScalarCannotCoerceInputLiteral,
                    scalarType.Name,
                    valueLiteral.Kind);
        if (error is not null)
        {
            errorBuilder.SetException(error);
        }
        return new LeafCoercionException(
            errorBuilder.Build(),
            scalarType);
    }

    public static LeafCoercionException ScalarCannotCoerceInputValue(
        ITypeDefinition scalarType,
        JsonElement inputValue,
        Exception? error = null)
    {
        var errorBuilder =
            ErrorBuilder.New()
                .SetMessage(
                    GraphQlTypeResources.ScalarCannotCoerceInputValue,
                    scalarType.Name,
                    inputValue.ValueKind);
        if (error is not null)
        {
            errorBuilder.SetException(error);
        }
        return new LeafCoercionException(
            errorBuilder.Build(),
            scalarType);
    }
}