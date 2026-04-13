using HotChocolate.Data.Filters;
using Metabase.Configuration;
using Metabase.Data;
using Metabase.GraphQl.Methods;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionManagedMethodFilterType
    : MethodFilterType
{
    protected override void Configure(
        IFilterInputTypeDescriptor<Method> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(InstitutionManagedMethodFilterType)[..^"FilterType".Length] + GraphQlConstants.FilterInputSuffix);
        descriptor.Field(x => x.Manager).Ignore();
    }
}