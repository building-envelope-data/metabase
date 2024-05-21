using HotChocolate.Data.Filters;
using Metabase.Data;

namespace Metabase.GraphQl.UserMethodDevelopers;

public sealed class UserMethodDeveloperFilterType
    : FilterInputType<UserMethodDeveloper>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<UserMethodDeveloper> descriptor
    )
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(x => x.Method);
        descriptor.Field(x => x.User);
        descriptor.Field(x => x.Pending);
    }
}