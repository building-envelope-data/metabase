using HotChocolate.Data.Filters;
using Metabase.Configuration;
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
        descriptor.Name(nameof(UserDevelopedMethodFilterType)[..^10] + GraphQlConstants.FilterInputSuffix);
        descriptor.Field(x => x.User).Ignore();
    }
}