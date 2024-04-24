using HotChocolate.Data.Filters;
using Metabase.Data;

namespace Metabase.GraphQl.Databases;

public sealed class DatabaseFilterType
    : FilterInputType<Database>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<Database> descriptor
    )
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(x => x.Id);
        descriptor.Field(x => x.Name);
        descriptor.Field(x => x.Description);
        descriptor.Field(x => x.Locator);
        descriptor.Field(x => x.Operator);
    }
}