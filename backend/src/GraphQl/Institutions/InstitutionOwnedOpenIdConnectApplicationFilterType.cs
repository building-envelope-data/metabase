using HotChocolate.Data.Filters;
using Metabase.Configuration;
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
        descriptor.Name(nameof(InstitutionOwnedOpenIdConnectApplicationFilterType)[..^10] + GraphQlConstants.FilterInputSuffix);
        descriptor.Field(x => x.Owner).Ignore();
    }
}