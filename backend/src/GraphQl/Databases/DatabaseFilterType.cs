using HotChocolate.Data.Filters;
using Metabase.Data;
using Metabase.GraphQl.Entities;

namespace Metabase.GraphQl.Databases;

public class DatabaseFilterType
    : EntityFilterType<Database>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<Database> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Field(x => x.Name);
        descriptor.Field(x => x.Description);
        descriptor.Field(x => x.Locator);
        descriptor.Field(x => x.Operator);
    }
}