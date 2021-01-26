using HotChocolate.Data.Filters;

namespace Metabase.GraphQl.Components
{
    public sealed class ComponentFilterType
      : FilterInputType<Data.Component>
    {
        protected override void Configure(
          IFilterInputTypeDescriptor<Data.Component> descriptor
          )
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Field(x => x.Id);
            descriptor.Field(x => x.Name);
            descriptor.Field(x => x.Abbreviation);
            descriptor.Field(x => x.Description);
            descriptor.Field(x => x.Categories);
            // TODO Allow filtering by Availability. How? See https://chillicream.com/docs/hotchocolate/fetching-data/filtering/#customization
        }
    }
}
