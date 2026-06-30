using HotChocolate.Data.Filters;
using Metabase.Data.OpenIdConnect;
using Metabase.GraphQl.OpenIdConnect.Applications;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionOwnedOpenIdConnectApplicationFilterType
    : OpenIdConnectApplicationFilterType
{
    protected override void Configure(
        IFilterInputTypeDescriptor<OpenIdConnectApplication> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(InstitutionOwnedOpenIdConnectApplicationFilterType)[..^"FilterType".Length] + GraphQlConstants.FilterInputSuffix);
        descriptor.Field(_ => _.Owner).Ignore();
    }
}
