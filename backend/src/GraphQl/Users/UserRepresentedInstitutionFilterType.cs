using HotChocolate.Data.Filters;
using Metabase.Configuration;
using Metabase.Data;
using Metabase.GraphQl.InstitutionRepresentatives;

namespace Metabase.GraphQl.Users;

public sealed class UserRepresentedInstitutionFilterType
    : InstitutionRepresentativeFilterType
{
    protected override void Configure(
        IFilterInputTypeDescriptor<InstitutionRepresentative> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(UserRepresentedInstitutionFilterType)[..^10] + GraphQlConstants.FilterInputSuffix);
        descriptor.Field(x => x.User).Ignore();
    }
}