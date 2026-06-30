using HotChocolate.Types;
using Metabase.Data.OpenIdConnect;

namespace Metabase.GraphQl.OpenIdConnect;

public sealed class OpenIdConnectSubjectType
    : UnionType<IOpenIdConnectSubject>
{
    protected override void Configure(IUnionTypeDescriptor descriptor)
    {
        descriptor.Name(nameof(IOpenIdConnectSubject)[1..]);
    }
}