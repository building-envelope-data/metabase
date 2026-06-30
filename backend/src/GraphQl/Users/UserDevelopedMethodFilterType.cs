using HotChocolate.Data.Filters;
using Metabase.Data;
using Metabase.GraphQl.UserMethodDevelopers;

namespace Metabase.GraphQl.Users;

public sealed class UserDevelopedMethodFilterType
    : UserMethodDeveloperFilterType
{
    protected override void Configure(
        IFilterInputTypeDescriptor<UserMethodDeveloper> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(UserDevelopedMethodFilterType)[..^"FilterType".Length] + GraphQlConstants.FilterInputSuffix);
        descriptor.Field(_ => _.User).Ignore();
    }
}
