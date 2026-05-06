using HotChocolate.Data.Sorting;
using Metabase.Data;
using Metabase.GraphQl.Entities;

namespace Metabase.GraphQl.Databases;

public class DatabaseSortType
    : AuditableEntitySortType<Database>
{
    protected override void Configure(
        ISortInputTypeDescriptor<Database> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Field(x => x.Name);
        descriptor.Field(x => x.Description);
        descriptor.Field(x => x.Locator);
    }
}