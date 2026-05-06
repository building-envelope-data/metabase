using System.Text.Json;
using HotChocolate.Language;
using HotChocolate.Text.Json;
using Microsoft.Extensions.DependencyInjection;

namespace HotChocolate.Types;

/// <summary>
/// <para>
/// The NonNegativeInt scalar type represents an unsigned 32‐bit numeric
/// non‐fractional value. Response formats that support an unsigned 32‐bit
/// integer or a number type should use that type to represent this scalar.
/// </para>
/// </summary>
public sealed class NonNegativeIntType
: IntegerTypeBase<uint>
{
    public const string ScalarName = "NonNegativeInt";

    /// <summary>
    /// Initializes a new instance of the <see cref="NonNegativeIntType"/> class.
    /// </summary>
    public NonNegativeIntType(uint min, uint max)
        : this(
            ScalarName,
            $"The `{ScalarName}` scalar type represents an unsigned 32-bit numeric non-fractional value.",
            min,
            max,
            BindingBehavior.Implicit)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NonNegativeIntType"/> class.
    /// </summary>
    public NonNegativeIntType(
        string name,
        string? description = null,
        uint min = uint.MinValue,
        uint max = uint.MaxValue,
        BindingBehavior bind = BindingBehavior.Explicit)
        : base(name, min, max, bind)
    {
        Description = description;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NonNegativeIntType"/> class.
    /// </summary>
    [ActivatorUtilitiesConstructor]
    public NonNegativeIntType()
        : this(uint.MinValue, uint.MaxValue)
    {
    }

    /// <inheritdoc />
    protected override uint OnCoerceInputLiteral(IntValueNode valueLiteral)
        => valueLiteral.ToUInt32();

    /// <inheritdoc />
    protected override uint OnCoerceInputValue(JsonElement inputValue)
        => inputValue.GetUInt32();

    /// <inheritdoc />
    protected override void OnCoerceOutputValue(uint runtimeValue, ResultElement resultValue)
        => resultValue.SetNumberValue(runtimeValue);

    /// <inheritdoc />
    protected override IValueNode OnValueToLiteral(uint runtimeValue)
        => new IntValueNode(runtimeValue);
}