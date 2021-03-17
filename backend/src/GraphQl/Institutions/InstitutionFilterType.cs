using HotChocolate.Data.Filters;

namespace Metabase.GraphQl.Institutions
{
    public sealed class InstitutionFilterType
      : FilterInputType<Data.Institution>
    {
        protected override void Configure(
          IFilterInputTypeDescriptor<Data.Institution> descriptor
          )
        {
            descriptor.BindFieldsExplicitly();
            descriptor.Field(x => x.Id);
            descriptor.Field(x => x.Name);
            descriptor.Field(x => x.Abbreviation);
            descriptor.Field(x => x.Description);
            descriptor.Field(x => x.WebsiteLocator);
        }
    }
}