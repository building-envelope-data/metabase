using HotChocolate.Data.Filters;
using Metabase.Data;

namespace Metabase.GraphQl.Methods;

public sealed class MethodDeveloperFilterType
    : FilterInputType<IMethodDeveloper>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<IMethodDeveloper> descriptor
    )
    {
        descriptor.BindFieldsExplicitly();
        // descriptor.Field(x => x.Institution);
        // descriptor.Field(x => x.User);
    }
}