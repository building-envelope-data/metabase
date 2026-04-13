using HotChocolate.Data.Sorting;
using Metabase.Data;
using Metabase.GraphQl.Entities;

namespace Metabase.GraphQl.Databases;

public sealed class DatabaseSortType
    : EntitySortType<Database>
{
    protected override void Configure(
        ISortInputTypeDescriptor<Database> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Field(x => x.Name);
        descriptor.Field(x => x.Description);
        descriptor.Field(x => x.Locator);
        descriptor.Field(x => x.Operator);
    }
}