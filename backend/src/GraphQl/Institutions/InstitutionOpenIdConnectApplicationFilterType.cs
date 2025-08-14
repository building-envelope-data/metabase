using HotChocolate.Data.Filters;
using Metabase.Configuration;
using Metabase.Data;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionOpenIdConnectApplicationFilterType
    : FilterInputType<InstitutionOpenIdConnectApplication>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<InstitutionOpenIdConnectApplication> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(InstitutionOpenIdConnectApplicationFilterType)[..^10] + GraphQlConfiguration.FilterInputSuffix);
        descriptor.Field(x => x.Application);
    }
}