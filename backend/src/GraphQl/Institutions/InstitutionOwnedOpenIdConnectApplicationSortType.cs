using HotChocolate.Data.Sorting;
using Metabase.Data.OpenIdConnect;
using Metabase.GraphQl.OpenIdConnect.Applications;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionOwnedOpenIdConnectApplicationSortType
    : OpenIdConnectApplicationSortType
{
    protected override void Configure(
        ISortInputTypeDescriptor<OpenIdConnectApplication> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(InstitutionOwnedOpenIdConnectApplicationSortType)[..^"SortType".Length] + GraphQlConstants.SortInputSuffix);
    }
}