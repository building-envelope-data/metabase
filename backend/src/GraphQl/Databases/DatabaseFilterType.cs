using HotChocolate.Data.Filters;

namespace Metabase.GraphQl.Databases
{
    public sealed class DatabaseFilterType
        : FilterInputType<Data.Database>
    {
        protected override void Configure(
            IFilterInputTypeDescriptor<Data.Database> descriptor
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
}