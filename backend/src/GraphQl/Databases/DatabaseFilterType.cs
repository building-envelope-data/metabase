using HotChocolate.Data.Filters;
using Metabase.Data;
using Metabase.GraphQl.Entities;

namespace Metabase.GraphQl.Databases;

public class DatabaseFilterType
    : AuditableEntityFilterType<Database>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<Database> descriptor
    )
    {
        base.Configure(descriptor);
        // TODO Remove Id, CreatedAt, and UpdatedAt once the base.Configure is respected.
        descriptor.Field(x => x.Id);
        descriptor.Field(x => x.CreatedAt);
        descriptor.Field(x => x.UpdatedAt);
        descriptor.Field(x => x.Name);
        descriptor.Field(x => x.Description);
        descriptor.Field(x => x.Locator);
        descriptor.Field(x => x.Operator);
    }
}