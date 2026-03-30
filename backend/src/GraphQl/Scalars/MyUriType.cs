using System;
using System.Text.Json;
using HotChocolate.Features;
using HotChocolate.Language;
using HotChocolate.Text.Json;
using Metabase.GraphQl;
using Microsoft.Extensions.DependencyInjection;

namespace HotChocolate.Types;

// Inspired by https://github.com/ChilliCream/graphql-platform/blob/main/src/HotChocolate/Core/src/Types/Types/Scalars/UriType.cs
/// <summary>
/// [RFC 3986](https://tools.ietf.org/html/rfc3986)
/// and
/// [RFC 3987](https://tools.ietf.org/html/rfc3987)
/// compliant
/// [absolute Uniform Resource Locator (URL)](https://tools.ietf.org/html/rfc3986#section-4.3)
/// string with optional
/// [fragment identifier](https://tools.ietf.org/html/rfc3986#section-3.5).
/// [Valid values are for example](https://datatracker.ietf.org/doc/html/rfc3986#section-1.1.2)
/// `ftp://ftp.is.co.za/rfc/rfc1808.txt`, `http://www.ietf.org/rfc/rfc2396.txt`,
/// `ldap://[2001:db8::7]/c=GB?objectClass?one`, `mailto:John.Doe@example.com`,
/// `news:comp.infosystems.www.servers.unix`, `tel:+1-816-555-1212`,
/// `telnet://192.0.2.16:80/`,
/// `urn:oasis:names:specification:docbook:dtd:xml:4.1.2`
/// 
/// See also
/// [URL Living Standard](https://url.spec.whatwg.org/#absolute-url-with-fragment-string)
/// and
/// [Identifying resources on the Web](https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/Identifying_resources_on_the_Web).
/// </summary>
/// <seealso href="https://tools.ietf.org/html/rfc3986">Specification</seealso>
public sealed class MyUriType : ScalarType<Uri, StringValueNode>
{
    private const string ScalarName = "Url";

    private const string SpecifiedByUri = "https://tools.ietf.org/html/rfc3986";

    public MyUriType(
        string name,
        string? description = null,
        BindingBehavior bind = BindingBehavior.Explicit)
        : base(name, bind)
    {
        Description = description;
        SpecifiedBy = new Uri(SpecifiedByUri);
    }

    /// </summary>
    [ActivatorUtilitiesConstructor]
    public MyUriType()
        : this(
            ScalarName,
            $"The `{ScalarName}` scalar type represents a Uniform Resource Identifier (URI) as defined by RFC 3986.",
            BindingBehavior.Implicit)
    {
    }

    /// <inheritdoc />
    protected override Uri OnCoerceInputLiteral(StringValueNode valueLiteral)
    {
        if (TryParseUri(valueLiteral.Value, out var value))
        {
            return value;
        }

        throw GraphQlThrowHelper.ScalarCannotCoerceInputLiteral(this, valueLiteral);
    }

    /// <inheritdoc />
    protected override Uri OnCoerceInputValue(JsonElement inputValue, IFeatureProvider context)
    {
        if (TryParseUri(inputValue.GetString()!, out var value))
        {
            return value;
        }

        throw GraphQlThrowHelper.ScalarCannotCoerceInputValue(this, inputValue);
    }

    /// <inheritdoc />
    protected override void OnCoerceOutputValue(Uri runtimeValue, ResultElement resultValue)
    {
        var serialized = runtimeValue.IsAbsoluteUri
            ? runtimeValue.AbsoluteUri
            : runtimeValue.ToString();
        resultValue.SetStringValue(serialized);
    }

    /// <inheritdoc />
    protected override StringValueNode OnValueToLiteral(Uri runtimeValue)
    {
        var value = runtimeValue.IsAbsoluteUri
            ? runtimeValue.AbsoluteUri
            : runtimeValue.ToString();
        return new StringValueNode(value);
    }

    private static bool TryParseUri(string value, out Uri uri)
    {
        if (!Uri.TryCreate(value, UriKind.RelativeOrAbsolute, out var parsedUri))
        {
            uri = null!;
            return false;
        }
        uri = parsedUri;
        return true;
    }
}