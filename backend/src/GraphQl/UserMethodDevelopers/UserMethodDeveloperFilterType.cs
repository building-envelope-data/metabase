using HotChocolate.Data.Filters;

namespace Metabase.GraphQl.UserMethodDevelopers;

public sealed class UserMethodDeveloperFilterType
    : FilterInputType<Data.UserMethodDeveloper>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<Data.UserMethodDeveloper> descriptor
    )
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(x => x.Method);
        descriptor.Field(x => x.User);
        descriptor.Field(x => x.Pending);
    }
}